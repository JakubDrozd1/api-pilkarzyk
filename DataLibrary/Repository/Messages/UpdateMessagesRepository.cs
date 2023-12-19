using System.Data;
using Dapper;
using DataLibrary.Entities;
using DataLibrary.IRepository.Messages;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository.Messages
{
    public class UpdateMessagesRepository(FbConnection dbConnection) : IUpdateMessagesRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;

        public async Task UpdateMessageAsync(MESSAGES message, FbTransaction? transaction = null)
        {
            var updateBuilder = new QueryBuilder<MESSAGES>()
               .Update("MESSAGES ", message)
               .Where("ID_MESSAGE = @ID_MESSAGE ");
            string updateQuery = updateBuilder.Build();
            FbConnection db = transaction?.Connection ?? _dbConnection;
            if (transaction == null && db.State != ConnectionState.Open)
            {
                await db.OpenAsync();
            }
            await db.ExecuteAsync(updateQuery, message, transaction);
        }
    }
}
