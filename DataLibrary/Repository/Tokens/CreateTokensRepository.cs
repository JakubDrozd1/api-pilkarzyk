using Dapper;
using DataLibrary.Helper;
using DataLibrary.IRepository.Tokens;
using DataLibrary.Model.DTO.Request.TableRequest;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository.Tokens
{
    public class CreateTokensRepository(FbConnection dbConnection, FbTransaction? fbTransaction) : ICreateTokensRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;
        private readonly FbTransaction? _fbTransaction = fbTransaction;

        public async Task AddAccessTokenAsync(GetAccessTokenRequest accessTokesInsert)
        {
            var insertBuilderAccessToken = new QueryBuilder<GetAccessTokenRequest>()
                .Insert("ACCESS_TOKENS ", accessTokesInsert);
            string insertBuilderAccessTokenQuery = insertBuilderAccessToken.Build();
            await _dbConnection.ExecuteAsync(insertBuilderAccessTokenQuery, accessTokesInsert, _fbTransaction);
        }

        public async Task AddRefreshTokenAsync(GetRefreshTokenRequest refreshTokenInsert)
        {
            var insertBuilderRefreshToken = new QueryBuilder<GetRefreshTokenRequest>()
                .Insert("REFRESH_TOKENS ", refreshTokenInsert);
            string insertBuilderRefreshTokenQuery = insertBuilderRefreshToken.Build();
            await _dbConnection.ExecuteAsync(insertBuilderRefreshTokenQuery, refreshTokenInsert, _fbTransaction);
        }
    }
}
