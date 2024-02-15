﻿using DataLibrary.Model.DTO.Request;
using DataLibrary.Model.DTO.Request.Pagination;
using DataLibrary.Model.DTO.Response;

namespace BLLLibrary.IService
{
    public interface IGroupInviteService
    {
        Task AddGroupInviteAsync(GetGroupInviteRequest getGroupInviteRequest);
        Task DeleteGroupInviteAsync(int groupInviteId);
        Task<List<GetGroupInviteResponse?>> GetGroupInviteByIdUserAsync(GetGroupInvitePaginationRequest getGroupInvitePaginationRequest);
        Task SaveChangesAsync();

    }
}
