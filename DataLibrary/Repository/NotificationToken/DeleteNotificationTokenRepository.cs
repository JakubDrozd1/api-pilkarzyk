using Dapper;
using DataLibrary.Entities;
using DataLibrary.Helper;
using System.Data;
using DataLibrary.IRepository.NotificationToken;
using FirebirdSql.Data.FirebirdClient;
using Microsoft.IdentityModel.Tokens;

namespace DataLibrary.Repository.NotificationToken
{
    public class DeleteNotificationTokenRepository(FbConnection dbConnection, FbTransaction? fbTransaction) : IDeleteNotificationTokenRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;
        private readonly FbTransaction? _fbTransaction = fbTransaction;

        public async Task DeleteNotificationTokenAsync(string token, int userId)
        {
            string WHERE = "1=1 ";
            DynamicParameters dynamicParameters = new();

            if (!token.IsNullOrEmpty())
            {
                WHERE += $"AND {nameof(NOTIFICATION_TOKENS.TOKEN)} = @Token ";
                dynamicParameters.Add("@Token", token);
            };

            WHERE += $"AND {nameof(NOTIFICATION_TOKENS.IDUSER)} = @IdUser ";
            dynamicParameters.Add("@IdUser", userId);

            if (_dbConnection.State != ConnectionState.Open)
            {
                await _dbConnection.OpenAsync();
            }
            try
            {
                var deleteBuilder = new QueryBuilder<NOTIFICATION_TOKENS>()
                    .Delete("NOTIFICATION_TOKENS ")
                    .Where(WHERE);
                string deleteQuery = deleteBuilder.Build();
                await _dbConnection.ExecuteAsync(deleteQuery, dynamicParameters, _fbTransaction);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

        public async Task DeleteNotificationTokenFromUsersAsync(string token)
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                await _dbConnection.OpenAsync();
            }
            try
            {
                var deleteBuilder = new QueryBuilder<NOTIFICATION_TOKENS>()
                    .Delete("NOTIFICATION_TOKENS ")
                    .Where("TOKEN = @Token ");
                string deleteQuery = deleteBuilder.Build();
                await _dbConnection.ExecuteAsync(deleteQuery, new { Token = token }, _fbTransaction);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
