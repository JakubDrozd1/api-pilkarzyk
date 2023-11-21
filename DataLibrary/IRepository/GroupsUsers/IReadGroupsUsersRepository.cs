using BLLLibrary.Model.DTO.Response;

namespace DataLibrary.IRepository
{
    public interface IReadGroupsUsersRepository
    {
        Task<List<GetGroupsWithUsersResponse>> GetAllUsersFromGroupAsync(int groupId);
        Task<List<GetGroupsWithUsersResponse>> GetAllGroupsFromUserAsync(int userId);
        Task<GetGroupsWithUsersResponse?> GetUserWithGroup(int groupId, int userId);
    }
}
