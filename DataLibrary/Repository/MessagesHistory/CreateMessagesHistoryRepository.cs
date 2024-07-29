using System.Data;
using Dapper;
using DataLibrary.Helper;
using DataLibrary.IRepository.Messages;
using DataLibrary.Model.DTO.Request.TableRequest;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository.MessagesHistory
{
    public class CreateMessagesHistoryRepository(FbConnection dbConnection, FbTransaction? fbTransaction) : ICreateMessagesHistoryRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;
        private readonly FbTransaction? _fbTransaction = fbTransaction;

        public async Task AddMessageHistoryAsync(GetMessageHistoryRequest message)
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                await _dbConnection.OpenAsync();
            }
            try
            {
                var insertBuilder = new QueryBuilder<GetMessageHistoryRequest>()
                    .Insert("MESSAGES_HISTORY ", message);
                string insertQuery = insertBuilder.Build();
                await _dbConnection.ExecuteAsync(insertQuery, message, _fbTransaction);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
