using Dapper;
using DataLibrary.Entities;
using DataLibrary.IRepository;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository
{
    public class DeleteGroupsUsersRepository(FbConnection dbConnection) : IDeleteGroupsUsersRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;


        public async Task DeleteUsersFromGroupAsync(int[] usersId, int groupId)
        {
            var deleteBuilder = new QueryBuilder<GroupsUsers>()
                .Delete("GROUPS_USERS")
                .Where("IDGROUP = @GroupId AND IDUSER IN (@UsersId)");
            string deleteQuery = deleteBuilder.Build();
            using FbConnection db = _dbConnection;
            await db.OpenAsync();
            await db.ExecuteAsync(deleteQuery, new { GroupId = groupId, UsersId = usersId });
        }

        public async Task DeleteAllUsersFromGroupAsync(int groupId)
        {
            var deleteBuilder = new QueryBuilder<GroupsUsers>()
                .Delete("GROUPS_USERS")
                .Where("IDGROUP = @GroupId");
            string deleteQuery = deleteBuilder.Build();
            using FbConnection db = _dbConnection;
            await db.OpenAsync();
            await db.ExecuteAsync(deleteQuery, new { GroupId = groupId });
        }

        public async Task DeleteAllGroupsFromUser(int userId)
        {
            var deleteBuilder = new QueryBuilder<GroupsUsers>()
                .Delete("GROUPS_USERS")
                .Where("IDUSER = @UserId");
            string deleteQuery = deleteBuilder.Build();
            using FbConnection db = _dbConnection;
            await db.OpenAsync();
            await db.ExecuteAsync(deleteQuery, new { UserId = userId });
        }
    }
}
