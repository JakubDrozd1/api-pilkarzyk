using DataLibrary.IRepository;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository
{
    public class UpdateGroupsUsersRepository(FbConnection dbConnection) : IUpdateGroupsUsersRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;

        public async Task UpdateGroupWithUsersAsync(int[] usersId, int groupId)
        {
            using FbConnection db = _dbConnection;
            DeleteGroupsUsersRepository deleteGroupsUsersRepository = new(db);
            CreateGroupsUsersRepository createGroupsUsersRepository = new(db);
            await db.OpenAsync();
            //await db.BeginTransactionAsync();
            await deleteGroupsUsersRepository.DeleteAllUsersFromGroupAsync(groupId);
            foreach (int userId in usersId)
            {
                await createGroupsUsersRepository.AddUserToGroupAsync(userId, groupId);
            }
        }

        public async Task UpdateUserWithGroupsAsync(int[] groupsId, int userId)
        {
            using FbConnection db = _dbConnection;
            DeleteGroupsUsersRepository deleteGroupsUsersRepository = new(db);
            CreateGroupsUsersRepository createGroupsUsersRepository = new(db);
            await db.OpenAsync();
            //await db.BeginTransactionAsync();
            await deleteGroupsUsersRepository.DeleteAllGroupsFromUser(userId);
            foreach (int groupId in groupsId)
            {
                await createGroupsUsersRepository.AddUserToGroupAsync(userId, groupId);
            }
        }
    }
}
