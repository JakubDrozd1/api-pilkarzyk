using BLLLibrary.IService;
using DataLibrary.Entities;
using DataLibrary.UoW;
using WebApi.Model.DTO.Request;

namespace BLLLibrary.Service
{
    public class UsersService(IUnitOfWork unitOfWork) : IUsersService
    {
        private readonly IUnitOfWork unitOfWork = unitOfWork;

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await unitOfWork.ReadUsersRepository.GetAllUsersAsync();
        }

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            return await unitOfWork.ReadUsersRepository.GetUserByIdAsync(userId);
        }

        public async Task AddUserAsync(UserRequest userRequest)
        {
            User user = new()
            {
                EMAIL = userRequest.Email,
                FIRSTNAME = userRequest.Firstname,
                SURNAME = userRequest.Surname,
                LOGIN = userRequest.Login,
                PASSWORD = userRequest.Password,
                PHONE_NUMBER = userRequest.PhoneNumber,
                ACCOUNT_TYPE = userRequest.AccountType,
            };
            await unitOfWork.CreateUsersRepository.AddUserAsync(user);
        }

        public async Task UpdateUserAsync(UserRequest userRequest)
        {
            User user = new()
            {
                EMAIL = userRequest.Email,
                FIRSTNAME = userRequest.Firstname,
                SURNAME = userRequest.Surname,
                LOGIN = userRequest.Login,
                PASSWORD = userRequest.Password,
                PHONE_NUMBER = userRequest.PhoneNumber,
                ACCOUNT_TYPE = userRequest.AccountType,
            };
            await unitOfWork.UpdateUsersRepository.UpdateUserAsync(user);
        }

        public async Task DeleteUserAsync(int userId)
        {
            await unitOfWork.DeleteUsersRepository.DeleteUserAsync(userId);
        }

        public async Task SaveChangesAsync()
        {
            await unitOfWork.SaveChangesAsync();
        }
    }
}
