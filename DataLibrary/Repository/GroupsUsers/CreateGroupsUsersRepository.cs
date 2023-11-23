using System.Data;
using Dapper;
using DataLibrary.Entities;
using DataLibrary.IRepository;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository
{
    public class CreateGroupsUsersRepository(FbConnection dbConnection) : ICreateGroupsUsersRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;

        public async Task AddUserToGroupAsync(int userId, int groupId, FbTransaction? transaction = null)
        {
            FbConnection db = transaction?.Connection ?? _dbConnection;
            if (transaction == null && db.State != ConnectionState.Open)
            {
                await db.OpenAsync();
            }
            var readGroupsRepository = new ReadGroupsRepository(db);
            var readUsersRepository = new ReadUsersRepository(db);
            var readGroupsUsersRepository = new ReadGroupsUsersRepository(db);
            _ = await readGroupsRepository.GetGroupByIdAsync(groupId, transaction) ?? throw new Exception("Group is null");
            _ = await readUsersRepository.GetUserByIdAsync(userId, transaction) ?? throw new Exception("User is null");
            if (await readGroupsUsersRepository.GetUserWithGroup(groupId, userId, transaction) != null)
            {
                throw new Exception("User is exist in this group");
            }
            GROUPS_USERS groupsUsers = new()
            {
                IDGROUP = groupId,
                IDUSER = userId,
            };
            var insertBuilder = new QueryBuilder<GROUPS_USERS>().Insert("GROUPS_USERS ", groupsUsers);
            string insertQuery = insertBuilder.Build();
            await db.ExecuteAsync(insertQuery, groupsUsers, transaction);
        }
    }
}
