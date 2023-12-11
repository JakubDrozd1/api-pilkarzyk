﻿using System.Data;
using DataLibrary.IRepository;
using DataLibrary.Model.DTO.Request;
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
                    GetUserGroupRequest getUserGroupRequest = new() { IdGroup = groupId, IdUser = userId};
                    await createGroupsUsersRepository.AddUserToGroupAsync(getUserGroupRequest, localTransaction);
                }
                localTransaction.Commit();
            }
            catch (Exception ex)
            {
                if (localTransaction != null)
                    localTransaction?.Rollback();
                throw new Exception($"Error while executing query: {ex.Message}");
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
                    GetUserGroupRequest getUserGroupRequest = new() { IdGroup = groupId, IdUser = userId };
                    await createGroupsUsersRepository.AddUserToGroupAsync(getUserGroupRequest, localTransaction);
                }
                localTransaction.Commit();
            }
            catch (Exception)
            {
                localTransaction?.Rollback();
                throw;
            }
        }
    }
}
