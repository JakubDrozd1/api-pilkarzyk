namespace DataLibrary.Helper.Notification
{
    public interface INotificationHub
    {
        Task SendMessage(int userId, int meetingId);

    }
}
