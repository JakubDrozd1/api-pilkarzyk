using DataLibrary.Entities;
using DataLibrary.Model.DTO.Request;
using DataLibrary.Model.DTO.Request.Pagination;
using DataLibrary.Model.DTO.Response;

namespace DataLibrary.IRepository.Users
{
    public interface IReadUsersRepository
    {
        Task<List<USERS>> GetAllUsersAsync(GetUsersPaginationRequest getUsersPaginationRequest);
        Task<USERS?> GetUserByIdAsync(int userId);
        Task<USERS?> GetUserByLoginAndPasswordAsync(GetUsersByLoginAndPasswordRequest getUsersByLoginAndPassword, USERS? user);
        Task<USERS?> GetUserByLoginAsync(string? login);
        Task<List<USERS>> GetAllUsersWithoutGroupAsync(GetUsersWithoutGroupPaginationRequest getUsersWithoutGroupPaginationRequest, List<GetGroupsUsersResponse> getGroupsUsersResponse);
        Task<USERS?> GetUserByEmailAsync(string email);
        Task<USERS?> GetUserByPhoneNumberAsync(int phoneNumber);
        Task<string?> GetSaltByUserId(int userId);
    }
}
