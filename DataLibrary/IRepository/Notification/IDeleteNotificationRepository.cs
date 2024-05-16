namespace DataLibrary.IRepository.Notification
{
    public interface IDeleteNotificationRepository
    {
        Task DeletaAllNotificationFromUser(int userId);

    }
}
