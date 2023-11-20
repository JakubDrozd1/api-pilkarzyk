using DataLibrary.IRepository;
using Dapper;
using FirebirdSql.Data.FirebirdClient;
using DataLibrary.Entities;

namespace DataLibrary.Repository
{
    public class ReadUsersRepository(FbConnection dbConnection) : IReadUsersRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;
        public async Task<List<User>> GetAllUsersAsync()
        {
            using FbConnection db = _dbConnection;
            await db.OpenAsync();
            var query = new QueryBuilder<User>()
                .Select("*")
                .From("USERS");
            return (await db.QueryAsync<User>(query.Build())).AsList();
        }

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            using FbConnection db = _dbConnection;
            await db.OpenAsync();
            var query = new QueryBuilder<User>()
                .Select("*")
                .From("USERS")
                .Where("ID_USER = @UserId");
            return await db.QueryFirstOrDefaultAsync<User>(query.Build(), new { UserId = userId });
        }
    }
}
