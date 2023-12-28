using System.Data;
using Dapper;
using DataLibrary.Entities;
using DataLibrary.Helper;
using DataLibrary.IRepository.Tokens;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository.Tokens
{
    public class ReadTokensRepository(FbConnection dbConnection, FbTransaction? fbTransaction) : IReadTokensRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;
        private readonly FbTransaction? _fbTransaction = fbTransaction;

        public async Task<ACCESS_TOKENS?> GetTokenByUserAsync(int userId)
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                await _dbConnection.OpenAsync();
            }
            try
            {
                var query = new QueryBuilder<ACCESS_TOKENS>()
                    .Select("* ")
                    .From("ACCESS_TOKENS ")
                    .Where("IDUSER = @UserId ");
                return await _dbConnection.QuerySingleOrDefaultAsync<ACCESS_TOKENS>(query.Build(), new { UserId = userId }, _fbTransaction);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

        public async Task<CLIENT_TOKENS?> GetClientTokenByUserAsync(string clientId)
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                await _dbConnection.OpenAsync();
            }
            try
            {
                string SELECT = $"{nameof(CLIENT_TOKENS.IDUSER)}, " +
                    $"{nameof(CLIENT_TOKENS.CLIENT_SECRET)}, " +
                    $"{nameof(CLIENT_TOKENS.REFRESH_TOKEN_TIME)}, " +
                    $"{nameof(CLIENT_TOKENS.GRANT_TYPE)}, " +
                    $"{nameof(CLIENT_TOKENS.ID_CLIENT)}, " +
                    $"{nameof(CLIENT_TOKENS.TOKEN_TIME)} ";
                var query = new QueryBuilder<CLIENT_TOKENS>()
                    .Select(SELECT)
                    .From(nameof(CLIENT_TOKENS))
                    .Where($"{nameof(CLIENT_TOKENS.ID_CLIENT)} = @ClientId ");
                return await _dbConnection.QuerySingleOrDefaultAsync<CLIENT_TOKENS>(query.Build(), new { ClientId = clientId }, _fbTransaction);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

        public async Task<REFRESH_TOKENS?> GetRefreshTokenAsync(string refreshToken)
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                await _dbConnection.OpenAsync();
            }
            try
            {
                string SELECT = $"{nameof(REFRESH_TOKENS.IDUSER)}, " +
                    $"{nameof(REFRESH_TOKENS.DATE_EXPIRE)}, " +
                    $"{nameof(REFRESH_TOKENS.REFRESH_TOKEN_VALUE)}, " +
                    $"{nameof(REFRESH_TOKENS.IDCLIENT)} ";
                var query = new QueryBuilder<REFRESH_TOKENS>()
                    .Select(SELECT)
                    .From(nameof(REFRESH_TOKENS))
                    .Where($"{nameof(REFRESH_TOKENS.REFRESH_TOKEN_VALUE)} = @RefreshToken");
                return await _dbConnection.QuerySingleOrDefaultAsync<REFRESH_TOKENS>(query.Build(), new { RefreshToken = refreshToken }, _fbTransaction);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
