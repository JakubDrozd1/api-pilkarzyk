using BLLLibrary.IService;
using DataLibrary.Entities;
using DataLibrary.Model.DTO.Request;
using DataLibrary.UoW;
using WebApi.Model.DTO.Request;

namespace BLLLibrary.Service
{
    public class UsersService(IUnitOfWork unitOfWork) : IUsersService
    {
        private readonly IUnitOfWork unitOfWork = unitOfWork;

        public async Task<List<USERS>> GetAllUsersAsync(GetUsersPaginationRequest getUsersPaginationRequest)
        {
            return await unitOfWork.ReadUsersRepository.GetAllUsersAsync(getUsersPaginationRequest);
        }

        public async Task<USERS?> GetUserByIdAsync(int userId)
        {
            return await unitOfWork.ReadUsersRepository.GetUserByIdAsync(userId);
        }

        public async Task AddUserAsync(GetUserRequest userRequest)
        {
            USERS user = new()
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

        public async Task UpdateUserAsync(GetUserRequest userRequest, int userId)
        {
            USERS user = new()
            {
                ID_USER = userId,
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
