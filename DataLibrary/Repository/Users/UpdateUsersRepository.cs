using DataLibrary.IRepository;
using Dapper;
using FirebirdSql.Data.FirebirdClient;
using DataLibrary.Entities;

namespace DataLibrary.Repository
{
    public class UpdateUsersRepository(FbConnection dbConnection) : IUpdateUsersRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;

        public async Task UpdateUserAsync(User user)
        {
            var updateBuilder = new QueryBuilder<User>()
                .Update("USERS", user)
                .Where("ID_USER = @UserId");
            string updateQuery = updateBuilder.Build();
            using FbConnection db = _dbConnection;
            await db.OpenAsync();
            await db.ExecuteAsync(updateQuery, user);
        }
    }
}
