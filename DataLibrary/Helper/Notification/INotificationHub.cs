using DataLibrary.Entities;
using DataLibrary.Model.DTO.Response;

namespace DataLibrary.Helper.Notification
{
    public interface INotificationHub
    {
        void SendMeetingNotification(GetMeetingGroupsResponse meeting, List<NOTIFICATION_TOKENS> tokens);
    }
}
