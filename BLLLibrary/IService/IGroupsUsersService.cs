﻿using BLLLibrary.Model.DTO.Response;

namespace BLLLibrary.IService
{
    public interface IGroupsUsersService
    {
        Task<List<GetGroupsWithUsersResponse>> GetAllGroupsFromUserAsync(int userId);
        Task<List<GetGroupsWithUsersResponse>> GetAllUsersFromGroupAsync(int groupId);
        Task<GetGroupsWithUsersResponse?> GetUserWithGroup(int groupId, int userId);
        Task DeleteUsersFromGroupAsync(int[] usersId, int groupId);
        Task DeleteAllUsersFromGroupAsync(int groupId);
        Task DeleteAllGroupsFromUser(int userId);
        Task UpdateUserWithGroupsAsync(int[] groupsId, int userId);
        Task UpdateGroupWithUsersAsync(int[] usersId, int groupId);
        Task AddUserToGroupAsync(int userId, int groupId);
    }
}
