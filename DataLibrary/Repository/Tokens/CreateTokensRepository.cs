using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Dapper;
using DataLibrary.Entities;
using DataLibrary.IRepository.Tokens;
using DataLibrary.Model.DTO.Request;
using DataLibrary.Model.DTO.Response;
using DataLibrary.Repository.Users;
using FirebirdSql.Data.FirebirdClient;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DataLibrary.Repository.Tokens
{
    public class CreateTokensRepository(IConfiguration configuration, FbConnection dbConnection) : ICreateTokensRepository
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly FbConnection dbConnection = dbConnection;
        private string grantType = "";

        public async Task<GetTokenResponse> GenerateJwtTokenAsync(GetTokenRequest tokenRequest, FbTransaction? transaction = null)
        {
            grantType = tokenRequest.Grant_type;
            FbConnection db = transaction?.Connection ?? dbConnection;
            ReadUsersRepository readUsersRepository = new(dbConnection);
            var parameters = new DynamicParameters();
            if (transaction == null && db.State != ConnectionState.Open)
            {
                await db.OpenAsync();
            }
            if (tokenRequest.Client_id == null)
            {
                throw new Exception($"client id is null");
            }
            using var localTransaction = transaction ?? await db.BeginTransactionAsync();
            switch (tokenRequest.Grant_type)
            {
                case "refresh_token":
                    if (tokenRequest.Refresh_token == null)
                    {
                        throw new Exception($"Invalid refresh token");
                    }
                    parameters.Add("@refreshToken", tokenRequest.Refresh_token);

                    var query = new QueryBuilder<REFRESH_TOKENS>()
                        .Select
                        (
                        $"{nameof(REFRESH_TOKENS.IDUSER)}, " +
                        $"{nameof(REFRESH_TOKENS.DATE_EXPIRE)}, " +
                        $"{nameof(REFRESH_TOKENS.REFRESH_TOKEN_VALUE)}, " +
                        $"{nameof(REFRESH_TOKENS.IDCLIENT)} "
                        )
                        .From(nameof(REFRESH_TOKENS))
                        .Where($"{nameof(REFRESH_TOKENS.REFRESH_TOKEN_VALUE)} = @refreshToken");

                    var refreshToken = await db.QuerySingleOrDefaultAsync<REFRESH_TOKENS>(query.Build(), parameters, localTransaction) ?? throw new Exception("Invalid refresh token");
                    var clientRefreshParams = await CheckClient(refreshToken.IDCLIENT, localTransaction) ?? throw new Exception($"Invalid client: {refreshToken.IDCLIENT}");
                    var userRefreshParams = await readUsersRepository.GetUserByIdAsync(refreshToken.IDUSER, localTransaction) ?? throw new Exception($"Invalid client: {refreshToken.IDUSER}");
                    var tokenExpired = refreshToken.DATE_EXPIRE < DateTime.Now;
                    if (tokenRequest.Client_id == null)
                    {
                        throw new Exception($"client id is null");
                    }
                    if (!tokenExpired)
                    {
                        return await GetTokenResponse(tokenRequest.Client_id, userRefreshParams, clientRefreshParams, localTransaction);
                    }
                    else
                    {
                        throw new Exception("Refresh token expired");
                    }

                case "password":
                    var clientPasswordParams = await CheckClient(tokenRequest.Client_id, localTransaction);
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
                    var user = await readUsersRepository.GetUserByLoginAndPasswordAsync(new GetUsersByLoginAndPassword()
                    {
                        Login = tokenRequest.Username,
                        Password = tokenRequest.Password
                    }, localTransaction);
                    if (user == null)
                    {
                        throw new Exception($"Invalid user: {user?.ID_USER}");
                    }
                    return await GetTokenResponse(tokenRequest.Client_id, user, clientPasswordParams, localTransaction);

                default:
                    throw new Exception($"Invalid grant type: {tokenRequest.Grant_type}");
            }
        }

        private async Task<GetTokenResponse> GetTokenResponse(string refreshToken, USERS user, CLIENT_TOKENS client, FbTransaction? transaction = null)
        {
            FbConnection db = transaction?.Connection ?? dbConnection;
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

            ACCESS_TOKENS accessTokesInsert = new()
            {
                ACCESS_TOKEN_VALUE = accessTokenValue,
                IDUSER = user.ID_USER,
                DATE_EXPIRE = expiresTime,
                IDCLIENT = refreshToken
            };
            REFRESH_TOKENS refreshTokenInsert = new()
            {
                DATE_EXPIRE = expiresRefreshTime,
                REFRESH_TOKEN_VALUE = refreshTokenValue,
                IDUSER = user.ID_USER,
                IDCLIENT = refreshToken
            };
            var insertBuilderAccessToken = new QueryBuilder<ACCESS_TOKENS>()
                .Insert("ACCESS_TOKENS ", accessTokesInsert);
            string insertBuilderAccessTokenQuery = insertBuilderAccessToken.Build();

            await db.ExecuteAsync(insertBuilderAccessTokenQuery, accessTokesInsert, transaction);

            var insertBuilderRefreshToken = new QueryBuilder<REFRESH_TOKENS>()
                .Insert("REFRESH_TOKENS ", refreshTokenInsert);
            string insertBuilderRefreshTokenQuery = insertBuilderRefreshToken.Build();
            await db.ExecuteAsync(insertBuilderRefreshTokenQuery, refreshTokenInsert, transaction);
            if (transaction != null)
            {
                await transaction.CommitAsync();
            }
            return new GetTokenResponse() { AccessToken = accessTokenValue, ExpiresIn = client.TOKEN_TIME ?? 7200, RefreshToken = refreshTokenValue };
        }

        private async Task<CLIENT_TOKENS?> CheckClient(string clientId, FbTransaction? transaction = null)
        {
            string? section = _configuration["client_id"];
            FbConnection db = transaction?.Connection ?? dbConnection;
            if (section == null || !section.Equals(clientId))
            {
                throw new Exception($"Invalid client: {clientId}");
            }
            else
            {
                var where = $"{nameof(CLIENT_TOKENS.ID_CLIENT)}=@client";
                var parameters = new DynamicParameters();
                parameters.Add("@client", clientId);

                var query = new QueryBuilder<CLIENT_TOKENS>()
                    .Select
                    (
                    $"{nameof(CLIENT_TOKENS.IDUSER)}, " +
                    $"{nameof(CLIENT_TOKENS.CLIENT_SECRET)}, " +
                    $"{nameof(CLIENT_TOKENS.REFRESH_TOKEN_TIME)}, " +
                    $"{nameof(CLIENT_TOKENS.GRANT_TYPE)}, " +
                    $"{nameof(CLIENT_TOKENS.ID_CLIENT)}, " +
                    $"{nameof(CLIENT_TOKENS.TOKEN_TIME)} "
                    )
                    .From(nameof(CLIENT_TOKENS))
                    .Where($"{nameof(CLIENT_TOKENS.ID_CLIENT)} = @client ");

                var clientToken = await db.QuerySingleOrDefaultAsync<CLIENT_TOKENS>(query.Build(), parameters, transaction);


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
    }
}
