using DataLibrary.IRepository;
using Dapper;
using FirebirdSql.Data.FirebirdClient;
using DataLibrary.Entities;

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
