using DataLibrary.Entities;
using DataLibrary.Model.DTO.Request.TableRequest;

namespace BLLLibrary.IService
{
    public interface INotificationService
    {
        Task AddNotificationToUserAsync(GetNotificationRequest getNotificationRequest);
        Task<NOTIFICATION?> GetAllNotificationFromUser(int userId);
        Task UpdateColumnNotificationAsync(GetUpdateNotificationRequest getUpdateNotificationRequest, int userId);
        Task DeletaAllNotificationFromUser(int userId);
        Task SaveChangesAsync();
    }
}
