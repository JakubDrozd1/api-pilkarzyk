using Dapper;
using DataLibrary.Entities;
using DataLibrary.IRepository;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository
{
    public class CreateUsersRepository(FbConnection dbConnection) : ICreateUsersRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;

        public async Task AddUserAsync(User user)
        {
            var insertBuilder = new QueryBuilder<User>()
                .Insert("USERS", user);
            string insertQuery = insertBuilder.Build();
            using FbConnection db = _dbConnection;
            await db.OpenAsync();
            await db.ExecuteAsync(insertQuery, user);
        }
    }
}
