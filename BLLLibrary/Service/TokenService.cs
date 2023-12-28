using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using BLLLibrary.IService;
using DataLibrary.Entities;
using DataLibrary.Model.DTO.Request;
using DataLibrary.Model.DTO.Response;
using DataLibrary.UoW;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BLLLibrary.Service
{
    public class TokenService(IUnitOfWork unitOfWork, IConfiguration configuration) : ITokenService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IConfiguration _configuration = configuration;
        private string grantType = "";

        public async Task<GetTokenResponse> GenerateJwtTokenAsync(GetTokenRequest tokenRequest)
        {
            await _unitOfWork.BeginTransactionAsync();
            grantType = tokenRequest.Grant_type;
            try
            {
                if (tokenRequest.Client_id == null)
                {
                    throw new Exception("ClientId is null");
                }
                switch (tokenRequest.Grant_type)
                {
                    case "refresh_token":
                        if (tokenRequest.Refresh_token == null)
                        {
                            throw new Exception($"Invalid refresh token");
                        }
                        REFRESH_TOKENS refreshToken = await _unitOfWork.ReadTokensRepository.GetRefreshTokenAsync(tokenRequest.Refresh_token) ?? throw new Exception("Invalid refresh token");
                        var clientRefreshParams = await CheckClient(refreshToken.IDCLIENT) ?? throw new Exception($"Invalid client: {refreshToken.IDCLIENT}");
                        var userRefreshParams = await _unitOfWork.ReadUsersRepository.GetUserByIdAsync(refreshToken.IDUSER) ?? throw new Exception($"Invalid client: {refreshToken.IDUSER}");
                        var tokenExpired = refreshToken.DATE_EXPIRE < DateTime.Now;
                        if (tokenRequest.Client_id == null)
                        {
                            throw new Exception($"client id is null");
                        }
                        if (!tokenExpired)
                        {
                            return await GetTokenResponse(tokenRequest.Client_id, userRefreshParams, clientRefreshParams);
                        }
                        else
                        {
                            throw new Exception("Refresh token expired");
                        }
                    case "password":
                        var clientPasswordParams = await CheckClient(tokenRequest.Client_id);
                        if (clientPasswordParams == null)
                        {
                            throw new Exception($"Invalid client: {tokenRequest.Client_id}");
                        }
                        else if (clientPasswordParams.CLIENT_SECRET != tokenRequest.Client_secret)
                        {
                            throw new Exception("Invalid client secret");
                        }
                        if (tokenRequest.Password == null) throw new Exception("Password is null");
                        if (tokenRequest.Username == null) throw new Exception("Username is null");
                        var userLogin = await _unitOfWork.ReadUsersRepository.GetUserByLoginAsync(tokenRequest.Username);
                        var user = await _unitOfWork.ReadUsersRepository.GetUserByLoginAndPasswordAsync(new GetUsersByLoginAndPasswordRequest()
                        {
                            Login = tokenRequest.Username,
                            Password = tokenRequest.Password
                        }, userLogin);
                        if (user == null)
                        {
                            throw new Exception($"Invalid user: {user?.ID_USER}");
                        }
                        return await GetTokenResponse(tokenRequest.Client_id, user, clientPasswordParams);
                    default:
                        throw new Exception($"Invalid grant type: {tokenRequest.Grant_type}");
                }
            }
            catch (Exception ex)
            {
                _unitOfWork.Dispose();
                throw new Exception($"{ex.Message}");
            }
        }

        private async Task<GetTokenResponse> GetTokenResponse(string refreshToken, USERS user, CLIENT_TOKENS client)
        {
            JwtSecurityTokenHandler tokenHandler = new();
            string? token = _configuration["TokenKey"] ?? throw new Exception("Key not found");
            byte[] key = Encoding.UTF8.GetBytes(token);
            string? audience = _configuration["Jwt:Audience"] ?? throw new Exception("Audience not found");
            string? issuer = _configuration["Jwt:Issuer"] ?? throw new Exception("Issuer not found");
            List<Claim> claims =
            [
                new Claim("idUser", user.ID_USER.ToString()),
                new Claim(ClaimTypes.Role, user.IS_ADMIN ? "Admin" : "User"),
                new Claim(ClaimTypes.Email, user.EMAIL),
                new Claim(ClaimTypes.Name, user.FIRSTNAME),
                new Claim(ClaimTypes.Surname, user.SURNAME),
                new Claim(JwtRegisteredClaimNames.Aud, audience),
                new Claim(JwtRegisteredClaimNames.Iss, issuer)
            ];
            var expiresTime = DateTime.Now.AddSeconds(client.TOKEN_TIME ?? 7200);
            var expiresRefreshTime = DateTime.Now.AddSeconds(client.REFRESH_TOKEN_TIME ?? 14400);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = audience,
                Audience = issuer,
                Subject = new ClaimsIdentity(claims),
                Expires = expiresTime,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenAccess = tokenHandler.CreateToken(tokenDescriptor);
            var accessTokenValue = tokenHandler.WriteToken(tokenAccess);
            string refreshTokenValue;
            using (var rngCryptoServiceProvider = RandomNumberGenerator.Create())
            {
                var randomBytes = new byte[30];
                rngCryptoServiceProvider.GetBytes(randomBytes);
                refreshTokenValue = Convert.ToBase64String(randomBytes);
            }
            GetAccessTokenRequest accessTokesInsert = new()
            {
                ACCESS_TOKEN_VALUE = accessTokenValue,
                IDUSER = user.ID_USER,
                DATE_EXPIRE = expiresTime,
                IDCLIENT = refreshToken
            };
            GetRefreshTokenRequest refreshTokenInsert = new()
            {
                DATE_EXPIRE = expiresRefreshTime,
                REFRESH_TOKEN_VALUE = refreshTokenValue,
                IDUSER = user.ID_USER,
                IDCLIENT = refreshToken
            };
            await _unitOfWork.CreateTokensRepository.AddAccessTokenAsync(accessTokesInsert);
            await _unitOfWork.CreateTokensRepository.AddRefreshTokenAsync(refreshTokenInsert);
            await _unitOfWork.SaveChangesAsync();
            return new GetTokenResponse() { AccessToken = accessTokenValue, ExpiresIn = client.TOKEN_TIME ?? 7200, RefreshToken = refreshTokenValue };
        }

        private async Task<CLIENT_TOKENS?> CheckClient(string clientId)
        {
            string? section = _configuration["client_id"];
            if (section == null || !section.Equals(clientId))
            {
                throw new Exception($"Invalid client: {clientId}");
            }
            else
            {
                var clientToken = await _unitOfWork.ReadTokensRepository.GetClientTokenByUserAsync(clientId);
                string requiredGrantType = "";
                switch (grantType)
                {
                    case "refresh_token":
                        requiredGrantType = "RefreshToken";
                        break;
                    case "password":
                        requiredGrantType = "UserCredentials";
                        break;
                }
                if (clientToken == null)
                {
                    throw new Exception($"Invalid client: {clientId}");
                }
                if (requiredGrantType == "" || !clientToken.GRANT_TYPE.ToString().Contains(requiredGrantType))
                {
                    throw new Exception($"Invalid grant type: {grantType}");
                }
                return clientToken;
            }
        }


        public async Task SaveChangesAsync()
        {
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
