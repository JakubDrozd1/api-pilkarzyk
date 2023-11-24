using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Dapper;
using DataLibrary.Entities;
using DataLibrary.IRepository;
using FirebirdSql.Data.FirebirdClient;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DataLibrary.Repository
{
    public class CreateTokensRepository(IConfiguration configuration, FbConnection dbConnection) : ICreateTokensRepository
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly FbConnection dbConnection = dbConnection;

        public async Task AddTokenAsync(TOKENS token, FbTransaction? transaction = null)
        {
            ReadUsersRepository readUsersRepository = new(dbConnection);
            USERS user = await readUsersRepository.GetUserByIdAsync(token.IDUSER) ?? throw new Exception("User is null");
            string secretKey = _configuration["JwtSettings:SecretKey"] ?? throw new Exception();
            string audience = _configuration["JwtSettings:Audience"] ?? throw new Exception();
            string issuer = _configuration["JwtSettings:Issuer"] ?? throw new Exception();
            string expiresIn = user.IS_ADMIN ? "never" : _configuration["JwtSettings:UserExpiresIn"] ?? throw new Exception();
            token.TOKEN_VALUE = GenerateJwtToken(secretKey, issuer, audience, token.IDUSER.ToString(), user, expiresIn);


            var insertBuilder = new QueryBuilder<GROUPS>()
                .Insert("TOKENS ", token);
            string insertQuery = insertBuilder.Build();
            FbConnection db = transaction?.Connection ?? dbConnection;

            if (transaction == null && db.State != ConnectionState.Open)
            {
                await db.OpenAsync();
            }

            await db.ExecuteAsync(insertQuery, token, transaction);
        }

        public static string GenerateJwtToken(string secretKey, string issuer, string audience, string userId, USERS user, string expiresIn)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Role, user.IS_ADMIN ? "Admin" : "User"),
                new Claim(ClaimTypes.Email, user.EMAIL),
                new Claim(ClaimTypes.GivenName, user.FIRSTNAME),
                new Claim(ClaimTypes.Surname, user.SURNAME ),
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: GetExpirationDate(expiresIn),
                signingCredentials: credentials
            );

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }

        public static ClaimsPrincipal ValidateToken(string token, string secretKey, string issuer, string audience)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = issuer,
                ValidAudience = audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
            };
            var principal = tokenHandler.ValidateToken(token, validationParameters, out _);

            return principal;
        }

        private static DateTime GetExpirationDate(string expiresIn)
        {
            if (expiresIn == "never")
            {
                return DateTime.MaxValue;
            }
            else
            {
                var expirationPeriod = TimeSpan.FromDays(double.Parse(expiresIn));
                return DateTime.UtcNow.Add(expirationPeriod);
            }
        }
    }
}
