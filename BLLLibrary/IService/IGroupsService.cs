﻿using DataLibrary.Entities;
using DataLibrary.Model.DTO.Request;
using DataLibrary.Model.DTO.Request.Pagination;
using DataLibrary.Model.DTO.Request.TableRequest;

namespace BLLLibrary.IService
{
    public interface IGroupsService
    {
        Task<List<GROUPS>> GetAllGroupsAsync(GetGroupsPaginationRequest getGroupsPaginationRequest);
        Task<GROUPS?> GetGroupByIdAsync(int groupId);
        Task AddGroupAsync(GetCreateGroupRequest groupRequest);
        Task UpdateGroupAsync(GetGroupRequest groupRequest, int groupId);
        Task DeleteGroupAsync(int groupId);
        Task<GROUPS?> GetGroupByNameAsync(string name);
        Task SaveChangesAsync();
    }
}
