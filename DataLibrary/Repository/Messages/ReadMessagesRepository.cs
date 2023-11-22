using System.Data;
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
            if (db.State != ConnectionState.Open)
                await db.OpenAsync();
            var query = new QueryBuilder<Message>()
                .Select("*")
                .From("MESSAGES");
            return (await db.QueryAsync<Message>(query.Build())).AsList();
        }

        public async Task<Message?> GetMessageByIdAsync(int messageId, FbTransaction? transaction = null)
        {
            var query = new QueryBuilder<Message>()
                .Select("*")
                .From("MESSAGES")
                .Where("ID_MESSAGE = @MessageId");
            FbConnection db = transaction?.Connection ?? _dbConnection;
            if (transaction == null && db.State != ConnectionState.Open)
            {
                await db.OpenAsync();
            }
            return await db.QuerySingleOrDefaultAsync<Message>(query.Build(), new { MessageId = messageId }, transaction);
        }
    }
}
