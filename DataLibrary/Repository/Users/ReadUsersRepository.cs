using System.Data;
using Dapper;
using DataLibrary.Entities;
using DataLibrary.IRepository;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository
{
    public class ReadUsersRepository(FbConnection dbConnection) : IReadUsersRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;

        public async Task<List<User>> GetAllUsersAsync()
        {
            using FbConnection db = _dbConnection;
            if (db.State != ConnectionState.Open)
                await db.OpenAsync();
            var query = new QueryBuilder<User>()
                .Select("*")
                .From("USERS");
            return (await db.QueryAsync<User>(query.Build())).AsList();
        }

        public async Task<User?> GetUserByIdAsync(int userId, FbTransaction? transaction = null)
        {
            var query = new QueryBuilder<User>()
                .Select("*")
                .From("USERS")
                .Where("ID_USER = @UserId");
            FbConnection db = transaction?.Connection ?? _dbConnection;
            if (transaction == null && db.State != ConnectionState.Open)
            {
                await db.OpenAsync();
            }
            return await db.QuerySingleOrDefaultAsync<User>(query.Build(), new { UserId = userId }, transaction);
        }
    }
}
