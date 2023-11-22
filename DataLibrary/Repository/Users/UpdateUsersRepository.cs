using System.Data;
using Dapper;
using DataLibrary.Entities;
using DataLibrary.IRepository;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository
{
    public class UpdateUsersRepository(FbConnection dbConnection) : IUpdateUsersRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;

        public async Task UpdateUserAsync(User user, FbTransaction? transaction = null)
        {
            var updateBuilder = new QueryBuilder<User>()
                .Update("USERS", user)
                .Where("ID_USER = @ID_USER");
            string updateQuery = updateBuilder.Build();
            FbConnection db = transaction?.Connection ?? _dbConnection;
            if (transaction == null && db.State != ConnectionState.Open)
            {
                await db.OpenAsync();
            }
            await db.ExecuteAsync(updateQuery, user, transaction);
        }
    }
}
