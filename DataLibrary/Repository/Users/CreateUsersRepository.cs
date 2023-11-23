using System.Data;
using Dapper;
using DataLibrary.Entities;
using DataLibrary.IRepository;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository
{
    public class CreateUsersRepository(FbConnection dbConnection) : ICreateUsersRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;

        public async Task AddUserAsync(USERS user, FbTransaction? transaction = null)
        {
            var insertBuilder = new QueryBuilder<USERS>()
                .Insert("USERS ", user);
            string insertQuery = insertBuilder.Build();
            FbConnection db = transaction?.Connection ?? _dbConnection;
            if (transaction == null && db.State != ConnectionState.Open)
            {
                await db.OpenAsync();
            }
            await db.ExecuteAsync(insertQuery, user, transaction);
        }
    }
}
