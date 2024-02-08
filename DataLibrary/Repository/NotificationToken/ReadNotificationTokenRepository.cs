using Dapper;
using System.Data;
using DataLibrary.Entities;
using DataLibrary.Helper;
using DataLibrary.IRepository.NotificationToken;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository.NotificationToken
{
    public class ReadNotificationTokenRepository(FbConnection dbConnection, FbTransaction? fbTransaction) : IReadNotificationTokenRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;
        private readonly FbTransaction? _fbTransaction = fbTransaction;

        public async Task<List<NOTIFICATION_TOKENS>> GetAllTokensFromUser(int userId)
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                await _dbConnection.OpenAsync();
            }
            try
            {
                var query = new QueryBuilder<NOTIFICATION_TOKENS>()
                    .Select("* ")
                    .From("NOTIFICATION_TOKENS ")
                    .Where("IDUSER = @IdUser ");
                return (await _dbConnection.QueryAsync<NOTIFICATION_TOKENS>(query.Build(), new { IdUser = userId }, _fbTransaction)).AsList();
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
