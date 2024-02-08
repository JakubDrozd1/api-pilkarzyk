using Dapper;
using DataLibrary.Helper;
using System.Data;
using DataLibrary.IRepository.NotificationToken;
using DataLibrary.Model.DTO.Request;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository.NotificationToken
{
    public class CreateNotificationTokenRepository(FbConnection dbConnection, FbTransaction? fbTransaction) : ICreateNotificationTokenRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;
        private readonly FbTransaction? _fbTransaction = fbTransaction;

        public async Task AddTokenToUserAsync(GetNotificationTokenRequest getNotificationTokenRequest)
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                await _dbConnection.OpenAsync();
            }
            try
            {
                var insertBuilder = new QueryBuilder<GetNotificationTokenRequest>()
                    .Insert("NOTIFICATION_TOKENS ", getNotificationTokenRequest);
                string insertQuery = insertBuilder.Build();
                await _dbConnection.ExecuteAsync(insertQuery, getNotificationTokenRequest, _fbTransaction);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
