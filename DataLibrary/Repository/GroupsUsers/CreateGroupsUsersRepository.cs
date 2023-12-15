using System.Data;
using Dapper;
using DataLibrary.Entities;
using DataLibrary.IRepository;
using DataLibrary.Model.DTO.Request;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository
{
    public class CreateGroupsUsersRepository(FbConnection dbConnection) : ICreateGroupsUsersRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;

        public async Task AddUserToGroupAsync(GetUserGroupRequest getUserGroupRequest, FbTransaction? transaction = null)
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
                var group = await readGroupsRepository.GetGroupByIdAsync(getUserGroupRequest.IdGroup, localTransaction) ?? throw new Exception("Group is null");
                var user = await readUsersRepository.GetUserByIdAsync(getUserGroupRequest.IdUser, localTransaction) ?? throw new Exception("User is null");
                if (await readGroupsUsersRepository.GetUserWithGroup(getUserGroupRequest.IdGroup, getUserGroupRequest.IdUser, localTransaction) != null)
                {
                    throw new Exception("User is already in this group");
                }
                GROUPS_USERS groupsUsers = new()
                {
                    IDGROUP = getUserGroupRequest.IdGroup,
                    IDUSER = getUserGroupRequest.IdUser,
                    ACCOUNT_TYPE = getUserGroupRequest.AccountType ?? 0
                };
                var insertBuilder = new QueryBuilder<GROUPS_USERS>().Insert("GROUPS_USERS ", groupsUsers);
                string insertQuery = insertBuilder.Build();
                await db.ExecuteAsync(insertQuery, groupsUsers, localTransaction);
                if (transaction == null)
                {
                    await localTransaction.CommitAsync();
                }
            }
            catch (Exception ex)
            {
                if (transaction == null)
                    localTransaction?.RollbackAsync();
                throw new Exception($"Error while executing query: {ex.Message}");
            }
        }
    }
}
