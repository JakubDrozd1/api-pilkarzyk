using Dapper;
using DataLibrary.Helper;
using System.Data;
using DataLibrary.IRepository.Notification;
using DataLibrary.Model.DTO.Request.TableRequest;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository.Notification
{
    internal class CreateNotificationRepository(FbConnection dbConnection, FbTransaction? fbTransaction) : ICreateNotificationRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;
        private readonly FbTransaction? _fbTransaction = fbTransaction;

        public async Task AddNotificationToUserAsync(GetNotificationRequest getNotificationRequest)
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                await _dbConnection.OpenAsync();
            }
            try
            {
                var insertBuilder = new QueryBuilder<GetNotificationRequest>()
                    .Insert("NOTIFICATION ", getNotificationRequest);
                string insertQuery = insertBuilder.Build();
                await _dbConnection.ExecuteAsync(insertQuery, getNotificationRequest, _fbTransaction);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
