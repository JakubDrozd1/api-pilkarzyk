using Dapper;
using DataLibrary.Entities;
using DataLibrary.Helper;
using DataLibrary.IRepository.GroupsLink;
using FirebirdSql.Data.FirebirdClient;
using System.Data;

namespace DataLibrary.Repository.GroupsLink
{
    public class DeleteGroupsUsersLinkRepository(FbConnection dbConnection, FbTransaction? fbTransaction) : IDeleteGroupsUsersLinkRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;
        private readonly FbTransaction? _fbTransaction = fbTransaction;
        public async Task DeleteGroupLinkAsync(int groupId, int userId)
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                await _dbConnection.OpenAsync();
            }
            try
            {
                var deleteBuilder = new QueryBuilder<GROUPS_USERS_LINK>()
                    .Delete("GROUPS_USERS_LINK ")
                    .Where("IDGROUP = @GroupId  AND IDUSER = @UserId");
                string deleteQuery = deleteBuilder.Build();
                await _dbConnection.ExecuteAsync(deleteQuery, new { GroupId = groupId, UserId = userId }, _fbTransaction);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
