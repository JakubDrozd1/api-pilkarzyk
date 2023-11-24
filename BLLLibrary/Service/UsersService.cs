using BLLLibrary.IService;
using DataLibrary.Entities;
using DataLibrary.Model.DTO.Request;
using DataLibrary.UoW;
using WebApi.Model.DTO.Request;
using FirebirdSql.Data.FirebirdClient;

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
            string salt = BCrypt.Net.BCrypt.GenerateSalt();
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(userRequest.Password, salt);
            USERS user = new()
            {
                EMAIL = userRequest.Email,
                FIRSTNAME = userRequest.Firstname,
                SURNAME = userRequest.Surname,
                LOGIN = userRequest.Login,
                PASSWORD = hashedPassword,
                PHONE_NUMBER = userRequest.PhoneNumber,
                IS_ADMIN = userRequest.IsAdmin,
                SALT = salt
            };
            await unitOfWork.CreateUsersRepository.AddUserAsync(user);
        }

        public async Task UpdateUserAsync(GetUserRequest userRequest, int userId)
        {
            string salt = BCrypt.Net.BCrypt.GenerateSalt();
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(userRequest.Password, salt);
            USERS user = new()
            {
                ID_USER = userId,
                EMAIL = userRequest.Email,
                FIRSTNAME = userRequest.Firstname,
                SURNAME = userRequest.Surname,
                LOGIN = userRequest.Login,
                PASSWORD = hashedPassword,
                PHONE_NUMBER = userRequest.PhoneNumber,
                IS_ADMIN = userRequest.IsAdmin,
                SALT = salt
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

        public async Task<USERS?> GetUserByLoginAndPasswordAsync(GetUsersByLoginAndPassword getUsersByLoginAndPassword, FbTransaction? transaction = null)
        {
            return await unitOfWork.ReadUsersRepository.GetUserByLoginAndPasswordAsync(getUsersByLoginAndPassword);
        }
    }
}
