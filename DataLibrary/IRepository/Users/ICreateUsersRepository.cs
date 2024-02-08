using DataLibrary.Model.DTO.Request;

namespace DataLibrary.IRepository.Users
{
    public interface ICreateUsersRepository
    {
        Task AddUserAsync(GetUserRequest user);
    }
}
