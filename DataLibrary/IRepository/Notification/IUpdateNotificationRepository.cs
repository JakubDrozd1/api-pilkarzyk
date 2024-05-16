using DataLibrary.Model.DTO.Request.TableRequest;

namespace DataLibrary.IRepository.Notification
{
    public interface IUpdateNotificationRepository
    {
        Task UpdateColumnNotificationAsync(GetUpdateNotificationRequest getUpdateNotificationRequest, int userId);

    }
}
