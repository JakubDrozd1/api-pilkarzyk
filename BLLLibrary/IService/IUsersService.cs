using DataLibrary.Entities;
using DataLibrary.Model.DTO.Request;
using WebApi.Model.DTO.Request;

namespace BLLLibrary.IService
{
    public interface IUsersService
    {
        Task<List<USERS>> GetAllUsersAsync(GetUsersPaginationRequest getUsersPaginationRequest);
        Task<USERS?> GetUserByIdAsync(int userId);
        Task AddUserAsync(GetUserRequest userRequest);
        Task UpdateUserAsync(GetUserRequest userRequest, int userId);
        Task DeleteUserAsync(int userId);
        Task SaveChangesAsync();
    }
}
