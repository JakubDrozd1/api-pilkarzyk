using DataLibrary.Entities;

namespace DataLibrary.IRepository
{
    public interface ICreateUsersRepository
    {
        Task AddUserAsync(User user);
    }
}
