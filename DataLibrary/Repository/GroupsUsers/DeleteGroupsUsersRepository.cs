using System.Data;
using Dapper;
using DataLibrary.Entities;
using DataLibrary.IRepository;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository
{
    public class DeleteGroupsUsersRepository(FbConnection dbConnection) : IDeleteGroupsUsersRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;

        public async Task DeleteUsersFromGroupAsync(int[] usersId, int groupId, FbTransaction? transaction = null)
        {
            var deleteBuilder = new QueryBuilder<GroupsUsers>()
                .Delete("GROUPS_USERS")
                .Where("IDGROUP = @GroupId AND IDUSER IN @UsersId");
            string deleteQuery = deleteBuilder.Build();
            FbConnection db = transaction?.Connection ?? _dbConnection;
            if (db.State != ConnectionState.Open && transaction == null)
            {
                await db.OpenAsync();
            }
            await db.ExecuteAsync(deleteQuery, new { GroupId = groupId, UsersId = usersId });

        }

        public async Task DeleteAllUsersFromGroupAsync(int groupId, FbTransaction? transaction = null)
        {
            var deleteBuilder = new QueryBuilder<GroupsUsers>()
                .Delete("GROUPS_USERS")
                .Where("IDGROUP = @GroupId");
            string deleteQuery = deleteBuilder.Build();
            FbConnection db = transaction?.Connection ?? _dbConnection;
            if (db.State != ConnectionState.Open && transaction == null)
            {
                await db.OpenAsync();
            }
            await db.ExecuteAsync(deleteQuery, new { GroupId = groupId }, transaction);
        }

        public async Task DeleteAllGroupsFromUser(int userId, FbTransaction? transaction = null)
        {
            var deleteBuilder = new QueryBuilder<GroupsUsers>()
                .Delete("GROUPS_USERS")
                .Where("IDUSER = @UserId");
            string deleteQuery = deleteBuilder.Build();
            FbConnection db = transaction?.Connection ?? _dbConnection;
            if (db.State != ConnectionState.Open && transaction == null)
            {
                await db.OpenAsync();
            }
            await db.ExecuteAsync(deleteQuery, new { UserId = userId }, transaction);
        }
    }
}
