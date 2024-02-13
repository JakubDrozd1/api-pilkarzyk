using DataLibrary.Entities;
using DataLibrary.Model.DTO.Response;

namespace DataLibrary.Helper.Notification
{
    public interface IFirebaseNotification
    {
        void SendMeetingNotification(GetMeetingGroupsResponse meeting, List<NOTIFICATION_TOKENS> tokens);

        void SendGroupNotification(GROUPS group, List<NOTIFICATION_TOKENS> tokens);
    }
}
