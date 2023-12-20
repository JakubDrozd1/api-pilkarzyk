using System.Data;
using Dapper;
using DataLibrary.Entities;
using DataLibrary.Helper;
using DataLibrary.IRepository.Messages;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository.Messages
{
    public class CreateMessagesRepository(FbConnection dbConnection) : ICreateMessagesRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;

        public async Task AddMessageAsync(MESSAGES message, FbTransaction? transaction = null)
        {
            var insertBuilder = new QueryBuilder<MESSAGES>()
                .Insert("MESSAGES ", message);
            string insertQuery = insertBuilder.Build();
            FbConnection db = transaction?.Connection ?? _dbConnection;
            if (transaction == null && db.State != ConnectionState.Open)
            {
                await db.OpenAsync();
            }
            await db.ExecuteAsync(insertQuery, message, transaction);
        }
    }
}
