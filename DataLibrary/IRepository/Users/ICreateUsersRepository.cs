using DataLibrary.Model.DTO.Request.TableRequest;

namespace DataLibrary.IRepository.Users
{
    public interface ICreateUsersRepository
    {
        Task AddUserAsync(GetUserRequest user);
    }
}
