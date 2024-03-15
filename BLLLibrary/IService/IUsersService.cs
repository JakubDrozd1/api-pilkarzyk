using DataLibrary.Entities;
using DataLibrary.Model.DTO.Request;
using DataLibrary.Model.DTO.Request.Pagination;
using DataLibrary.Model.DTO.Request.TableRequest;

namespace BLLLibrary.IService
{
    public interface IUsersService
    {
        Task<List<USERS>> GetAllUsersAsync(GetUsersPaginationRequest getUsersPaginationRequest);
        Task<USERS?> GetUserByLoginAndPasswordAsync(GetUsersByLoginAndPasswordRequest getUsersByLoginAndPassword);
        Task<USERS?> GetUserByIdAsync(int userId);
        Task AddUserAsync(GetUserRequest userRequest);
        Task UpdateUserAsync(GetUserRequest userRequest, int userId);
        Task DeleteUserAsync(int userId);
        Task<List<USERS>> GetAllUsersWithoutGroupAsync(GetUsersWithoutGroupPaginationRequest getUsersWithoutGroupPaginationRequest);
        Task<USERS?> GetUserByEmailAsync(string email);
        Task UpdateColumnUserAsync(GetUpdateUserRequest getUpdateUserRequest, int userId);
        Task SaveChangesAsync();
        Task ChangePassword(GetUsersByLoginAndPasswordRequest getUsersByLoginAndPassword);
    }
}
