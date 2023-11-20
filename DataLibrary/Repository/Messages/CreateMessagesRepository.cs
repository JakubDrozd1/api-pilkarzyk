using Dapper;
using DataLibrary.Entities;
using DataLibrary.IRepository;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository.Messages
{
    public class CreateMessagesRepository(FbConnection dbConnection) : ICreateMessagesRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;

        public async Task AddMessageAsync(Message message)
        {
            var insertBuilder = new QueryBuilder<Message>()
                            .Insert("MESSAGES", message);
            string insertQuery = insertBuilder.Build();
            using FbConnection db = _dbConnection;
            await db.OpenAsync();
            await db.ExecuteAsync(insertQuery, message);
        }
    }
}
