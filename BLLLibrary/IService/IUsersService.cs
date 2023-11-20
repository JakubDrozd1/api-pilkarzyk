using DataLibrary.Entities;
using WebApi.Model.DTO.Request;

namespace BLLLibrary.IService
{
    public interface IUsersService
    {
        Task<List<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(int userId);
        Task AddUserAsync(UserRequest userRequest);
        Task UpdateUserAsync(UserRequest userRequest);
        Task DeleteUserAsync(int userId);
        Task SaveChangesAsync();
    }
}
