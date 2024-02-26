using Dapper;
using DataLibrary.Helper;
using System.Data;
using DataLibrary.IRepository.ChatMessages;
using DataLibrary.Model.DTO.Request;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository.ChatMessages
{
    public class CreateChatMessagesRepository(FbConnection dbConnection, FbTransaction? fbTransaction) : ICreateChatMessagesRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;
        private readonly FbTransaction? _fbTransaction = fbTransaction;

        public async Task AddMessageToChat(GetChatMessageRequest getChatMessageRequest)
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                await _dbConnection.OpenAsync();
            }
            try
            {
                var insertBuilder = new QueryBuilder<GetChatMessageRequest>()
                    .Insert("CHAT_MESSAGES ", getChatMessageRequest);
                string insertQuery = insertBuilder.Build();
                await _dbConnection.ExecuteAsync(insertQuery, getChatMessageRequest, _fbTransaction);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
