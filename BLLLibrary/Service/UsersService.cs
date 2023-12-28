using BLLLibrary.IService;
using DataLibrary.Entities;
using DataLibrary.IRepository.Users;
using DataLibrary.Model.DTO.Request;
using DataLibrary.Model.DTO.Request.Pagination;
using DataLibrary.Model.DTO.Response;
using DataLibrary.Repository.GroupsUsers;
using DataLibrary.Repository.Users;
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
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                USERS? userLogin = await _unitOfWork.ReadUsersRepository.GetUserByLoginAsync(userRequest.LOGIN);
                USERS? userEmail = await _unitOfWork.ReadUsersRepository.GetUserByEmailAsync(userRequest.EMAIL);
                USERS? userPhone = await _unitOfWork.ReadUsersRepository.GetUserByPhoneNumberAsync(userRequest.PHONE_NUMBER);

                if (userLogin != null)
                {
                    throw new Exception("Acount with login already exists");
                }
                if (userEmail != null)
                {
                    throw new Exception("Acount with email already exists");
                }
                if (userPhone != null)
                {
                    throw new Exception("Acount with phone number already exists");
                }
                string salt = BCrypt.Net.BCrypt.GenerateSalt();
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(userRequest.USER_PASSWORD, salt);
                userRequest.USER_PASSWORD = hashedPassword;
                userRequest.SALT = salt;
                await _unitOfWork.CreateUsersRepository.AddUserAsync(userRequest);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _unitOfWork.Dispose();
                throw new Exception($"{ex.Message}");
            }
        }

        public async Task UpdateUserAsync(GetUserRequest userRequest, int userId)
        {
            string salt = BCrypt.Net.BCrypt.GenerateSalt();
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(userRequest.USER_PASSWORD, salt);
            USERS user = new()
            {
                ID_USER = userId,
                EMAIL = userRequest.EMAIL,
                FIRSTNAME = userRequest.FIRSTNAME,
                SURNAME = userRequest.SURNAME,
                LOGIN = userRequest.LOGIN,
                USER_PASSWORD = hashedPassword,
                PHONE_NUMBER = userRequest.PHONE_NUMBER,
                IS_ADMIN = userRequest.IS_ADMIN,
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

        public async Task<USERS?> GetUserByLoginAndPasswordAsync(GetUsersByLoginAndPasswordRequest getUsersByLoginAndPassword)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var user = await _unitOfWork.ReadUsersRepository.GetUserByLoginAsync(getUsersByLoginAndPassword.Login) ?? throw new Exception("User is null");
                return await _unitOfWork.ReadUsersRepository.GetUserByLoginAndPasswordAsync(getUsersByLoginAndPassword, user);
            }
            catch (Exception ex)
            {
                _unitOfWork.Dispose();
                throw new Exception($"{ex.Message}");
            }
        }

        public async Task<List<USERS>> GetAllUsersWithoutGroupAsync(GetUsersWithoutGroupPaginationRequest getUsersWithoutGroupPaginationRequest)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                GetUsersGroupsPaginationRequest getUsersGroupsPaginationRequest = new()
                {
                    IdGroup = getUsersWithoutGroupPaginationRequest.IdGroup,
                    Page = getUsersWithoutGroupPaginationRequest.Page,
                    OnPage = getUsersWithoutGroupPaginationRequest.OnPage,
                };
                var getGroupsUsersResponse = await _unitOfWork.ReadGroupsUsersRepository.GetListGroupsUserAsync(getUsersGroupsPaginationRequest);
                return await _unitOfWork.ReadUsersRepository.GetAllUsersWithoutGroupAsync(getUsersWithoutGroupPaginationRequest, getGroupsUsersResponse);
            }
            catch (Exception ex)
            {
                _unitOfWork.Dispose();
                throw new Exception($"{ex.Message}");
            }
        }

        public async Task<USERS?> GetUserByEmailAsync(string email)
        {
            return await _unitOfWork.ReadUsersRepository.GetUserByEmailAsync(email);
        }

        public async Task UpdateColumnUserAsync(GetUpdateUserRequest getUpdateUserRequest, int userId)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                string salt = await _unitOfWork.ReadUsersRepository.GetSaltByUserId(userId) ?? throw new Exception("Salt is null");
                await _unitOfWork.UpdateUsersRepository.UpdateColumnUserAsync(getUpdateUserRequest, userId, salt);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _unitOfWork.Dispose();
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
