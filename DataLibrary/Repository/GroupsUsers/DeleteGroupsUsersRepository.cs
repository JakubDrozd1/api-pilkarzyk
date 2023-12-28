using System.Data;
using Dapper;
using DataLibrary.Entities;
using DataLibrary.Helper;
using DataLibrary.IRepository.GroupsUsers;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository.GroupsUsers
{
    public class DeleteGroupsUsersRepository(FbConnection dbConnection, FbTransaction? fbTransaction) : IDeleteGroupsUsersRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;
        private readonly FbTransaction? _fbtransaction = fbTransaction;

        public async Task DeleteUsersFromGroupAsync(int[] usersId, int groupId)
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                await _dbConnection.OpenAsync();
            }
            try
            {
                var deleteBuilder = new QueryBuilder<GROUPS_USERS>()
                    .Delete("GROUPS_USERS ")
                    .Where("IDGROUP = @GroupId AND IDUSER IN @UsersId ");
                string deleteQuery = deleteBuilder.Build();
                await _dbConnection.ExecuteAsync(deleteQuery, new { GroupId = groupId, UsersId = usersId }, _fbtransaction);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

        public async Task DeleteAllUsersFromGroupAsync(int groupId)
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                await _dbConnection.OpenAsync();
            }
            try
            {
                var deleteBuilder = new QueryBuilder<GROUPS_USERS>()
                    .Delete("GROUPS_USERS ")
                    .Where("IDGROUP = @GroupId ");
                string deleteQuery = deleteBuilder.Build();
                await _dbConnection.ExecuteAsync(deleteQuery, new { GroupId = groupId }, _fbtransaction);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

        public async Task DeleteAllGroupsFromUser(int userId)
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                await _dbConnection.OpenAsync();
            }
            try
            {
                var deleteBuilder = new QueryBuilder<GROUPS_USERS>()
                    .Delete("GROUPS_USERS ")
                    .Where("IDUSER = @UserId ");
                string deleteQuery = deleteBuilder.Build();
                await _dbConnection.ExecuteAsync(deleteQuery, new { UserId = userId }, _fbtransaction);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
