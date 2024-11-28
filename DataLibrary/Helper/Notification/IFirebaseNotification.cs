using DataLibrary.Entities;

namespace DataLibrary.Helper.Notification
{
    public interface IFirebaseNotification
    {
        Task SendMeetingNotification(List<NOTIFICATION_TOKENS> tokens, string title, string body, int IdMeeting);
        Task SendNotification(List<NOTIFICATION_TOKENS> tokens, string title, string body);
        Task SendTeamNotification(List<NOTIFICATION_TOKENS> tokens, string title, string body, int idMeeting);
        Task SendGroupNotification(List<NOTIFICATION_TOKENS> tokens, string title, string body, int idGroup);

    }
}
