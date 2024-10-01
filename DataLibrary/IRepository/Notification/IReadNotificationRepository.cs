using DataLibrary.Entities;
using DataLibrary.Model.DTO.Request.Pagination;
using DataLibrary.Model.DTO.Response;

namespace DataLibrary.IRepository.Notification
{
    public interface IReadNotificationRepository
    {
        Task<NOTIFICATION?> GetAllNotificationFromUser(int userId);
        Task<List<GetNotificationMessageResponse>> GetAllNotificationMessageFromUser(GetNotificationMessagePaginationRequest getNotificationMessagePaginationRequest);

    }
}
