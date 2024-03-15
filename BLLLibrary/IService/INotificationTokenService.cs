using DataLibrary.Entities;
using DataLibrary.Model.DTO.Request.TableRequest;

namespace BLLLibrary.IService
{
    public interface INotificationTokenService
    {
        Task<List<NOTIFICATION_TOKENS>> GetAllTokensFromUser(int userId);
        Task AddTokenToUserAsync(GetNotificationTokenRequest getNotificationTokenRequest);
        Task DeleteNotificationTokenAsync(string token, int userId);
        Task SaveChangesAsync();
    }
}
