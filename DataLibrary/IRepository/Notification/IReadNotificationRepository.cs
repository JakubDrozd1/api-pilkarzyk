using DataLibrary.Entities;

namespace DataLibrary.IRepository.Notification
{
    public interface IReadNotificationRepository
    {
        Task<NOTIFICATION?> GetAllNotificationFromUser(int userId);
        Task<List<NOTIFICATION_MESSAGES>> GetAllNotificationMessageFromUser(int userId);

    }
}
