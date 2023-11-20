using Dapper;
using FirebirdSql.Data.FirebirdClient;
using DataLibrary.IRepository.Users;
using DataLibrary.Entities;

namespace DataLibrary.Repository.Users
{
    public class DeleteUsersRepository(FbConnection dbConnection) : IDeleteUsersRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;

        public async Task DeleteUserAsync(int userId)
        {
            var deleteBuilder = new QueryBuilder<User>()
                .Delete("USERS")
                .Where("ID_USER = @UserId");
            string deleteQuery = deleteBuilder.Build();
            using FbConnection db = _dbConnection;
            await db.OpenAsync();
            await db.ExecuteAsync(deleteQuery, new { UserId = userId });
        }
    }
}
