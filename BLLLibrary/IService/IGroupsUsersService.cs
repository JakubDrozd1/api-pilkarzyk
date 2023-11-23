using BLLLibrary.Model.DTO.Response;
using DataLibrary.Model.DTO.Request;

namespace BLLLibrary.IService
{
    public interface IGroupsUsersService
    {
        Task<List<GetGroupsUsersResponse>> GetListGroupsUserAsync(GetUsersGroupsPaginationRequest getUsersGroupsPaginationRequest);
        Task<GetGroupsUsersResponse?> GetUserWithGroup(int groupId, int userId);
        Task DeleteUsersFromGroupAsync(int[] usersId, int groupId);
        Task DeleteAllUsersFromGroupAsync(int groupId);
        Task DeleteAllGroupsFromUser(int userId);
        Task UpdateUserWithGroupsAsync(int[] groupsId, int userId);
        Task UpdateGroupWithUsersAsync(int[] usersId, int groupId);
        Task AddUserToGroupAsync(int userId, int groupId);
    }
}
