using DataLibrary.Entities;

namespace DataLibrary.IRepository
{
    public interface IUpdateUsersRepository
    {
        Task UpdateUserAsync(User user);
    }
}
