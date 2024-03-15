using DataLibrary.Entities;
using DataLibrary.Model.DTO.Request.Pagination;
using DataLibrary.Model.DTO.Request.TableRequest;
using DataLibrary.Model.DTO.Response;

namespace DataLibrary.IRepository.GroupInvite
{
    public interface IReadGroupInviteRepository
    {
        Task<List<GetGroupInviteResponse?>> GetGroupInviteByIdUserAsync(GetGroupInvitePaginationRequest getGroupInvitePaginationRequest);
        Task<GROUP_INVITE?> GetGroupInviteByIdAsync(int groupInviteId);
        Task<GROUP_INVITE?> GetLastAddedInvite(GetGroupInviteRequest getGroupInviteRequest);
    }
}
