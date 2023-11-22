using System.Data;
using DataLibrary.IRepository;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository
{
    public class UpdateGroupsUsersRepository(FbConnection dbConnection) : IUpdateGroupsUsersRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;

        public async Task UpdateGroupWithUsersAsync(int[] usersId, int groupId, FbTransaction? transaction = null)
        {
            using FbConnection db = transaction?.Connection ?? _dbConnection;
            if (transaction == null && db.State != ConnectionState.Open)
            {
                await db.OpenAsync();
            }
            var deleteGroupsUsersRepository = new DeleteGroupsUsersRepository(db);
            var createGroupsUsersRepository = new CreateGroupsUsersRepository(db);
            using var localTransaction = transaction ?? db.BeginTransaction();
            try
            {
                await deleteGroupsUsersRepository.DeleteAllUsersFromGroupAsync(groupId, localTransaction);
                foreach (int userId in usersId)
                {
                    await createGroupsUsersRepository.AddUserToGroupAsync(userId, groupId, localTransaction);
                }
                localTransaction.Commit();
            }
            catch (Exception)
            {
                localTransaction.Rollback();
                throw;
            }
        }

        public async Task UpdateUserWithGroupsAsync(int[] groupsId, int userId, FbTransaction? transaction = null)
        {
            using FbConnection db = transaction?.Connection ?? _dbConnection;
            if (transaction == null && db.State != ConnectionState.Open)
            {
                await db.OpenAsync();
            }
            var deleteGroupsUsersRepository = new DeleteGroupsUsersRepository(db);
            var createGroupsUsersRepository = new CreateGroupsUsersRepository(db);
            using var localTransaction = transaction ?? db.BeginTransaction();
            try
            {
                await deleteGroupsUsersRepository.DeleteAllGroupsFromUser(userId, localTransaction);
                foreach (int groupId in groupsId)
                {
                    await createGroupsUsersRepository.AddUserToGroupAsync(userId, groupId, localTransaction);
                }
                localTransaction.Commit();
            }
            catch (Exception)
            {
                localTransaction.Rollback();
                throw;
            }
        }
    }
}
