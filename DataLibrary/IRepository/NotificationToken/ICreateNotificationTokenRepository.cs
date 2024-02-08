using DataLibrary.Model.DTO.Request;

namespace DataLibrary.IRepository.NotificationToken
{
    public interface ICreateNotificationTokenRepository
    {
        Task AddTokenToUserAsync(GetNotificationTokenRequest getNotificationTokenRequest);

    }
}
