using DataLibrary.Entities;

namespace DataLibrary.IRepository.NotificationToken
{
    public interface IReadNotificationTokenRepository
    {
        Task<List<NOTIFICATION_TOKENS>> GetAllTokensFromUser(int userId);
    }
}
