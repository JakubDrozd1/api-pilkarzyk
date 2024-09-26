using DataLibrary.Entities;
using DataLibrary.Model.DTO.Request.Pagination;
using DataLibrary.Model.DTO.Request.TableRequest;
using DataLibrary.Model.DTO.Response;

namespace BLLLibrary.IService
{
    public interface INotificationService
    {
        Task AddNotificationToUserAsync(GetNotificationRequest getNotificationRequest);
        Task<NOTIFICATION?> GetAllNotificationFromUser(int userId);
        Task<List<GetNotificationMessageResponse>> GetAllNotificationMessageFromUser(GetNotificationMessagePaginationRequest getNotificationMessagePaginationRequest);
        Task UpdateColumnNotificationAsync(GetUpdateNotificationRequest getUpdateNotificationRequest, int userId);
        Task DeletaAllNotificationFromUser(int userId);
        Task SaveChangesAsync();
    }
}
