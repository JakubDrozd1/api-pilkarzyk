using BLLLibrary.IService;
using DataLibrary.Entities;
using DataLibrary.Model.DTO.Request;
using DataLibrary.UoW;

namespace BLLLibrary.Service
{
    public class NotificationTokenService(IUnitOfWork unitOfWork) : INotificationTokenService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task AddTokenToUserAsync(GetNotificationTokenRequest getNotificationTokenRequest)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _unitOfWork.DeleteNotificationTokenRepository.DeleteNotificationTokenFromUsersAsync(getNotificationTokenRequest.TOKEN);
                await _unitOfWork.CreateNotificationTokenRepository.AddTokenToUserAsync(getNotificationTokenRequest);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollBackTransactionAsync();
                throw new Exception($"{ex.Message}");
            }
        }

        public async Task DeleteNotificationTokenAsync(string token, int userId)
        {
            await _unitOfWork.DeleteNotificationTokenRepository.DeleteNotificationTokenAsync(token, userId);
        }

        public async Task<List<NOTIFICATION_TOKENS>> GetAllTokensFromUser(int userId)
        {
            return await _unitOfWork.ReadNotificationTokenRepository.GetAllTokensFromUser(userId);
        }

        public async Task SaveChangesAsync()
        {
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
