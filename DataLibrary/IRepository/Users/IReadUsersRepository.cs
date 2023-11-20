using DataLibrary.Entities;

namespace DataLibrary.IRepository
{
    public interface IReadUsersRepository
    {
        Task<List<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(int userId);
    }
}
