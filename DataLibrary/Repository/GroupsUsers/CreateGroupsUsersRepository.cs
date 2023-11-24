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
            var localTransaction = transaction ?? await db.BeginTransactionAsync();
            try
            {
                var readGroupsRepository = new ReadGroupsRepository(db);
                var readUsersRepository = new ReadUsersRepository(db);
                var readGroupsUsersRepository = new ReadGroupsUsersRepository(db);
                var group = await readGroupsRepository.GetGroupByIdAsync(groupId, localTransaction) ?? throw new Exception("Group is null");
                var user = await readUsersRepository.GetUserByIdAsync(userId, localTransaction) ?? throw new Exception("User is null");
                if (await readGroupsUsersRepository.GetUserWithGroup(groupId, userId, localTransaction) != null)
                {
                    throw new Exception("User is already in this group");
                }
                GROUPS_USERS groupsUsers = new()
                {
                    IDGROUP = groupId,
                    IDUSER = userId,
                    ACCOUNT_TYPE = 0
                };
                var insertBuilder = new QueryBuilder<GROUPS_USERS>().Insert("GROUPS_USERS ", groupsUsers);
                string insertQuery = insertBuilder.Build();
                await db.ExecuteAsync(insertQuery, groupsUsers, localTransaction);
                if (transaction == null)
                {
                    localTransaction.Commit();
                }
            }
            catch (Exception ex)
            {
                if (transaction == null)
                    localTransaction?.Rollback();
                throw new Exception($"Error while executing query: {ex.Message}");
            }
        }
    }
}
