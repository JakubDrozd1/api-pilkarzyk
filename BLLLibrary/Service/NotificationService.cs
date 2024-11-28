using BLLLibrary.IService;
using DataLibrary.Entities;
using DataLibrary.Model.DTO.Request.Pagination;
using DataLibrary.Model.DTO.Request.TableRequest;
using DataLibrary.Model.DTO.Response;
using DataLibrary.UoW;

namespace BLLLibrary.Service
{
    public class NotificationService(IUnitOfWork unitOfWork) : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task AddNotificationMessageToUserAsync(GetNotificationMessageRequest getNotificationRequest)
        {
            await _unitOfWork.CreateNotificationRepository.AddNotificationMessageToUserAsync(getNotificationRequest);
        }

        public async Task AddNotificationToUserAsync(GetNotificationRequest getNotificationRequest)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _unitOfWork.DeleteNotificationRepository.DeletaAllNotificationFromUser(getNotificationRequest.IDUSER);
                await _unitOfWork.CreateNotificationRepository.AddNotificationToUserAsync(getNotificationRequest);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollBackTransactionAsync();
                throw new Exception($"{ex.Message}");
            }
        }

        public async Task DeletaAllNotificationFromUser(int userId)
        {
            await _unitOfWork.DeleteNotificationRepository.DeletaAllNotificationFromUser(userId);
        }

        public async Task<NOTIFICATION?> GetAllNotificationFromUser(int userId)
        {
            return await _unitOfWork.ReadNotificationRepository.GetAllNotificationFromUser(userId);
        }

        public async Task<List<GetNotificationMessageResponse>> GetAllNotificationMessageFromUser(GetNotificationMessagePaginationRequest getNotificationMessagePaginationRequest)
        {
            return await _unitOfWork.ReadNotificationRepository.GetAllNotificationMessageFromUser(getNotificationMessagePaginationRequest);
        }

        public async Task SaveChangesAsync()
        {
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateColumnNotificationAsync(GetUpdateNotificationRequest getUpdateNotificationRequest, int userId)
        {
            await _unitOfWork.UpdateNotificationRepository.UpdateColumnNotificationAsync(getUpdateNotificationRequest, userId);
        }

        public async Task UpdateColumnNotificationMessageAsync(GetUpdateNotificationMessageRequest getUpdateNotificationMessageRequest, int notificationMessageId)
        {
            await _unitOfWork.UpdateNotificationRepository.UpdateColumnNotificationMessageAsync(getUpdateNotificationMessageRequest, notificationMessageId);
        }
    }
}
