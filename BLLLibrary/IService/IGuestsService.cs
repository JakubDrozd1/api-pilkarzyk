using DataLibrary.Entities;
using DataLibrary.Model.DTO.Request.TableRequest;
using DataLibrary.Model.DTO.Response;

namespace BLLLibrary.IService
{
    public interface IGuestsService
    {
        Task AddGuestsAsync(GetGuestRequest getGuestRequest);
        Task DeleteGuestsAsync(int guestsId);
        Task<List<GetGuestsMeetingsResponse?>> GetAllGuestFromMeetingAsync(int meetingId);
        Task<GUESTS?> GetGuestByIdAsync(int guestId);
        Task UpdateGuestsAsync(GetGuestRequest getGuestRequest, int guestId);
        Task SaveChangesAsync();

    }
}
