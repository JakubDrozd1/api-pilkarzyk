using BLLLibrary.IService;
using DataLibrary.Entities;
using DataLibrary.Model.DTO.Request;
using DataLibrary.UoW;

namespace BLLLibrary.Service
{
    public class UsersService(IUnitOfWork unitOfWork) : IUsersService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<List<USERS>> GetAllUsersAsync(GetUsersPaginationRequest getUsersPaginationRequest)
        {
            return await _unitOfWork.ReadUsersRepository.GetAllUsersAsync(getUsersPaginationRequest);
        }

        public async Task<USERS?> GetUserByIdAsync(int userId)
        {
            return await _unitOfWork.ReadUsersRepository.GetUserByIdAsync(userId);
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
                USER_PASSWORD = hashedPassword,
                PHONE_NUMBER = userRequest.PhoneNumber,
                IS_ADMIN = userRequest.IsAdmin,
                SALT = salt
            };
            await _unitOfWork.CreateUsersRepository.AddUserAsync(user);
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
                USER_PASSWORD = hashedPassword,
                PHONE_NUMBER = userRequest.PhoneNumber,
                IS_ADMIN = userRequest.IsAdmin,
                SALT = salt
            };
            await _unitOfWork.UpdateUsersRepository.UpdateUserAsync(user);
        }

        public async Task DeleteUserAsync(int userId)
        {
            await _unitOfWork.DeleteUsersRepository.DeleteUserAsync(userId);
        }

        public async Task SaveChangesAsync()
        {
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<USERS?> GetUserByLoginAndPasswordAsync(GetUsersByLoginAndPassword getUsersByLoginAndPassword)
        {
            return await _unitOfWork.ReadUsersRepository.GetUserByLoginAndPasswordAsync(getUsersByLoginAndPassword);
        }

        public async Task<List<USERS>> GetAllUsersWithoutGroupAsync(GetUsersWithoutGroupPaginationRequest getUsersWithoutGroupPaginationRequest)
        {
            return await _unitOfWork.ReadUsersRepository.GetAllUsersWithoutGroupAsync(getUsersWithoutGroupPaginationRequest);
        }

        public async Task<USERS?> GetUserByEmailAsync(string email)
        {
            return await _unitOfWork.ReadUsersRepository.GetUserByEmailAsync(email);
        }

        public async Task UpdateColumnUserAsync(GetUpdateUserRequest getUpdateUserRequest, int userId)
        {
            await _unitOfWork.UpdateUsersRepository.UpdateColumnUserAsync(getUpdateUserRequest, userId);
        }
    }
}
