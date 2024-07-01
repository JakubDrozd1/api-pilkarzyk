using DataLibrary.Entities;
using DataLibrary.Model.DTO.Response;

namespace DataLibrary.IRepository.Guests
{
    public interface IReadGuestsRepository
    {
        Task<List<GetGuestsMeetingsResponse?>> GetAllGuestFromMeetingAsync(int meetingId);
        Task<GUESTS?> GetGuestByIdAsync(int guestId);
    }
}
