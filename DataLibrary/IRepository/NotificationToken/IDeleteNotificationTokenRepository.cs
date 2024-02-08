namespace DataLibrary.IRepository.NotificationToken
{
    public interface IDeleteNotificationTokenRepository
    {
        Task DeleteNotificationTokenFromUsersAsync(string token);

        Task DeleteNotificationTokenAsync(string token, int userId);

    }
}
