using DataLibrary.Entities;
using DataLibrary.Model.DTO.Request;
using FirebirdSql.Data.FirebirdClient;
using WebApi.Model.DTO.Request;

namespace BLLLibrary.IService
{
    public interface IUsersService
    {
        Task<List<USERS>> GetAllUsersAsync(GetUsersPaginationRequest getUsersPaginationRequest);
        Task<USERS?> GetUserByLoginAndPasswordAsync(GetUsersByLoginAndPassword getUsersByLoginAndPassword);
        Task<USERS?> GetUserByIdAsync(int userId);
        Task AddUserAsync(GetUserRequest userRequest);
        Task UpdateUserAsync(GetUserRequest userRequest, int userId);
        Task DeleteUserAsync(int userId);
        Task<List<USERS>> GetAllUsersWithoutGroupAsync(GetUsersWithoutGroupPaginationRequest getUsersWithoutGroupPaginationRequest);
        Task<USERS?> GetUserByEmailAsync(string email);
        Task UpdateColumnUserAsync(GetUpdateUserRequest getUpdateUserRequest, int userId);
        Task SaveChangesAsync();
    }
}
