using System.Data;
using Dapper;
using DataLibrary.Entities;
using DataLibrary.IRepository;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository
{
    public class DeleteUsersRepository(FbConnection dbConnection) : IDeleteUsersRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;

        public async Task DeleteUserAsync(int userId, FbTransaction? transaction = null)
        {
            var deleteBuilder = new QueryBuilder<USERS>()
                .Delete("USERS ")
                .Where("ID_USER = @UserId ");
            string deleteQuery = deleteBuilder.Build();
            FbConnection db = transaction?.Connection ?? _dbConnection;
            if (transaction == null && db.State != ConnectionState.Open)
            {
                await db.OpenAsync();
            }
            await db.ExecuteAsync(deleteQuery, new { UserId = userId }, transaction);
        }
    }
}
