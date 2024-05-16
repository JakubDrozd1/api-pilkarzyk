using DataLibrary.Entities;

namespace DataLibrary.IRepository.Notification
{
    public interface IReadNotificationRepository
    {
        Task<NOTIFICATION?> GetAllNotificationFromUser(int userId);

    }
}
