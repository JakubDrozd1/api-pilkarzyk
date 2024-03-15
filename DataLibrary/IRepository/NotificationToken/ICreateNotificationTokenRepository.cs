using DataLibrary.Model.DTO.Request.TableRequest;

namespace DataLibrary.IRepository.NotificationToken
{
    public interface ICreateNotificationTokenRepository
    {
        Task AddTokenToUserAsync(GetNotificationTokenRequest getNotificationTokenRequest);

    }
}
