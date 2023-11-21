using Dapper;
using DataLibrary.Entities;
using DataLibrary.IRepository;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository
{
    public class UpdateUsersRepository(FbConnection dbConnection) : IUpdateUsersRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;

        public async Task UpdateUserAsync(User user)
        {
            var updateBuilder = new QueryBuilder<User>()
                .Update("USERS", user)
                .Where("ID_USER = @ID_USER");
            string updateQuery = updateBuilder.Build();
            using FbConnection db = _dbConnection;
            await db.OpenAsync();
            var affected = await db.ExecuteAsync(updateQuery, user);
        }
    }
}
