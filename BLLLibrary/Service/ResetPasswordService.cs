using BLLLibrary.IService;
using DataLibrary.Entities;
using DataLibrary.Model.DTO.Request;
using DataLibrary.Model.DTO.Response;
using DataLibrary.UoW;

namespace BLLLibrary.Service
{
    public class ResetPasswordService(IUnitOfWork unitOfWork) : IResetPasswordService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<RESET_PASSWORD?> AddResetPasswordAsync(string email)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var user = await _unitOfWork.ReadUsersRepository.GetUserByEmailAsync(email) ?? throw new Exception("User is null");
                await _unitOfWork.CreateResetPasswordRepository.AddResetPasswordAsync(new GetResetPasswordRequest()
                {
                    IDUSER = user.ID_USER
                });
                await _unitOfWork.SaveChangesAsync();
                return await GetLastAdded(user.ID_USER);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollBackTransactionAsync();
                throw new Exception($"{ex.Message}");
            }
        }

        public async Task<RESET_PASSWORD?> GetLastAdded(int userId)
        {
            return await _unitOfWork.ReadResetPasswordRepository.GetLastAdded(userId);
        }

        public async Task<GetResetPasswordResponse?> GetResetPasswordByIdAsync(int passwordResetId)
        {
            return await _unitOfWork.ReadResetPasswordRepository.GetResetPasswordByIdAsync(passwordResetId);
        }
    }
}
