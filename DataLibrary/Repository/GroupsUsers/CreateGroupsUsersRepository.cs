using Dapper;
using DataLibrary.Entities;
using DataLibrary.IRepository;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository
{
    public class CreateGroupsUsersRepository(FbConnection dbConnection) : ICreateGroupsUsersRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;

        public async Task AddUserToGroupAsync(int userId, int groupId)
        {
            ReadUsersRepository readUsersRepository = new(_dbConnection);
            ReadGroupsRepository readGroupsRepository = new(_dbConnection);
            _ = await readGroupsRepository.GetGroupByIdAsync(groupId) ?? throw new Exception("Group is null");
            _ = await readUsersRepository.GetUserByIdAsync(userId) ?? throw new Exception("User is null");
            GroupsUsers groupsUsers = new()
            {
                IDGROUP = groupId,
                IDUSER = userId,
            };
            var insertBuilder = new QueryBuilder<GroupsUsers>()
                    .Insert("GROUPS_USERS", groupsUsers);
            string insertQuery = insertBuilder.Build();
            await using FbConnection db = _dbConnection;
            await db.OpenAsync();
            await db.ExecuteAsync(insertQuery, groupsUsers);
        }
    }
}
