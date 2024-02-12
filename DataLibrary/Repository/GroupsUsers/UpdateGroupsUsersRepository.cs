using Dapper;
using DataLibrary.Entities;
using DataLibrary.Helper;
using System.Data;
using DataLibrary.IRepository.GroupsUsers;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository.GroupsUsers
{
    public class UpdateGroupsUsersRepository(FbConnection dbConnection, FbTransaction? fbTransaction) : IUpdateGroupsUsersRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;
        private readonly FbTransaction? _fbTransaction = fbTransaction;
        public async Task UpdateGroupUserAsync(GROUPS_USERS groupUsers)
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                await _dbConnection.OpenAsync();
            }
            try
            {
                var updateBuilder = new QueryBuilder<GROUPS_USERS>()
                    .Update("GROUPS_USERS ", groupUsers)
                    .Where("IDUSER = @IDUSER AND IDGROUP = @IDGROUP");
                string updateQuery = updateBuilder.Build();
                await _dbConnection.ExecuteAsync(updateQuery, groupUsers, _fbTransaction);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
