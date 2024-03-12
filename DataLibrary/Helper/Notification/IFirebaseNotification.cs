using DataLibrary.Entities;
using DataLibrary.Model.DTO.Request;
using DataLibrary.Model.DTO.Response;

namespace DataLibrary.Helper.Notification
{
    public interface IFirebaseNotification
    {
        void SendMeetingNotification(GetMeetingGroupsResponse meeting, List<NOTIFICATION_TOKENS> tokens);

        void SendGroupNotification(GROUPS group, List<NOTIFICATION_TOKENS> tokens);

        void SendMessageNotificationAsync(GetMeetingGroupsResponse meeting, GetMessageRequest getMessageRequest, List<NOTIFICATION_TOKENS> tokens);

        void SendNotificationToUserTeamAsync(string? teamName, List<NOTIFICATION_TOKENS> tokens);

    }
}
