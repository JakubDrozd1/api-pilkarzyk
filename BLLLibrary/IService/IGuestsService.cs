using DataLibrary.Entities;
using DataLibrary.Model.DTO.Request.TableRequest;

namespace BLLLibrary.IService
{
    public interface IGuestsService
    {
        Task AddGuestsAsync(GetGuestRequest getGuestRequest);
        Task DeleteGuestsAsync(int guestsId);
        Task<List<GUESTS?>> GetAllGuestFromMeetingAsync(int meetingId);
        Task<GUESTS?> GetGuestByIdAsync(int guestId);
        Task UpdateGuestsAsync(GetGuestRequest getGuestRequest, int guestId);
        Task SaveChangesAsync();

    }
}
