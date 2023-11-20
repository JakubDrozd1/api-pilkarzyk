using Dapper;
using DataLibrary.Entities;
using DataLibrary.IRepository.Messages;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository.Messages
{
    public class DeleteMessagesRepository(FbConnection dbConnection) : IDeleteMessagesRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;

        public async Task DeleteMessageAsync(int messageId)
        {
            var deleteBuilder = new QueryBuilder<Message>()
                .Delete("MESSAGES")
                .Where("ID_MESSAGES = @MessageId");
            string deleteQuery = deleteBuilder.Build();
            using FbConnection db = _dbConnection;
            await db.OpenAsync();
            await db.ExecuteAsync(deleteQuery, new { MessageId = messageId });
        }
    }
}
