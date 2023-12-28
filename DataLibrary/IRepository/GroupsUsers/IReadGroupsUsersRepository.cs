using DataLibrary.Model.DTO.Request.Pagination;
using DataLibrary.Model.DTO.Response;

namespace DataLibrary.IRepository.GroupsUsers
{
    public interface IReadGroupsUsersRepository
    {
        Task<List<GetGroupsUsersResponse>> GetListGroupsUserAsync(GetUsersGroupsPaginationRequest getUsersGroupsRequest);
        Task<GetGroupsUsersResponse?> GetUserWithGroup(int groupId, int userId);
    }
}
