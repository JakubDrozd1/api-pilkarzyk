using DataLibrary.Entities;
using DataLibrary.Helper;
using System.Data;
using DataLibrary.IRepository.Notification;
using FirebirdSql.Data.FirebirdClient;
using Dapper;

namespace DataLibrary.Repository.Notification
{
    public class DeleteNotificationRepository(FbConnection dbConnection, FbTransaction? fbTransaction) : IDeleteNotificationRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;
        private readonly FbTransaction? _fbTransaction = fbTransaction;
        public async Task DeletaAllNotificationFromUser(int userId)
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                await _dbConnection.OpenAsync();
            }
            try
            {
                var deleteBuilder = new QueryBuilder<NOTIFICATION>()
                    .Delete("NOTIFICATION ")
                    .Where("IDUSER = @UserId ");
                string deleteQuery = deleteBuilder.Build();
                await _dbConnection.ExecuteAsync(deleteQuery, new { UserId = userId }, _fbTransaction);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
