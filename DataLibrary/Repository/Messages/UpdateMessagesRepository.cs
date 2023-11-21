using Dapper;
using DataLibrary.Entities;
using DataLibrary.IRepository;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository
{
    public class UpdateMessagesRepository(FbConnection dbConnection) : IUpdateMessagesRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;

        public async Task UpdateMessageAsync(Message message)
        {
            var updateBuilder = new QueryBuilder<Message>()
               .Update("MESSAGES", message)
               .Where("ID_MESSAGE = @ID_MESSAGE");
            string updateQuery = updateBuilder.Build();
            using FbConnection db = _dbConnection;
            await db.OpenAsync();
            await db.ExecuteAsync(updateQuery, message);
        }
    }
}
