using DataLibrary.Model.DTO.Request.TableRequest;

namespace DataLibrary.IRepository.Notification
{
    public interface ICreateNotificationRepository
    {
        Task AddNotificationToUserAsync(GetNotificationRequest getNotificationRequest);

    }
}
