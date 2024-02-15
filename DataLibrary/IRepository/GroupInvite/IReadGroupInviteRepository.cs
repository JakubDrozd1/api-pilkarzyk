using DataLibrary.Model.DTO.Request.Pagination;
using DataLibrary.Model.DTO.Response;

namespace DataLibrary.IRepository.GroupInvite
{
    public interface IReadGroupInviteRepository
    {
        Task<List<GetGroupInviteResponse?>> GetGroupInviteByIdUserAsync(GetGroupInvitePaginationRequest getGroupInvitePaginationRequest);
    }
}
