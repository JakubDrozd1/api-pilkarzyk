using Dapper;
using DataLibrary.Entities;
using DataLibrary.IRepository;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository
{
    public class ReadMessagesRepository(FbConnection dbConnection) : IReadMessagesRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;

        public async Task<List<Message>> GetAllMessagesAsync()
        {
            using FbConnection db = _dbConnection;
            await db.OpenAsync();
            var query = new QueryBuilder<Message>()
                .Select("*")
                .From("MESSAGES");
            return (await db.QueryAsync<Message>(query.Build())).AsList();
        }

        public async Task<Message?> GetMessageByIdAsync(int messageId)
        {
            using FbConnection db = _dbConnection;
            await db.OpenAsync();
            var query = new QueryBuilder<Message>()
                .Select("*")
                .From("MESSAGES")
                .Where("ID_MESSAGE = @MessageId");
            return await db.QueryFirstOrDefaultAsync<Message>(query.Build(), new { MessageId = messageId });
        }
    }
}
