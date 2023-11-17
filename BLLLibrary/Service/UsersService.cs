using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLLLibrary.IService;
using DataLibrary;
using DataLibrary.Entities;
using DataLibrary.IRepository;
using DataLibrary.UoW;

namespace BLLLibrary.Service
{
    public class UsersService(IUnitOfWork unitOfWork) : IUsersService
    {
        private readonly IUnitOfWork unitOfWork = unitOfWork;

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await unitOfWork.UsersRepository.GetAllUsersAsync();
        }

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            return await unitOfWork.UsersRepository.GetUserByIdAsync(userId);
        }

        public async Task AddUserAsync(User user)
        {
            await unitOfWork.UsersRepository.AddUserAsync(user);
        }

        public async Task UpdateUserAsync(User user)
        {
            await unitOfWork.UsersRepository.UpdateUserAsync(user);
        }

        public async Task DeleteUserAsync(int userId)
        {
            await unitOfWork.UsersRepository.DeleteUserAsync(userId);
        }

        public async Task SaveChangesAsync()
        {
            await unitOfWork.SaveChangesAsync();
        }
    }
}
