using DataLibrary.Entities;

namespace DataLibrary.IRepository.Guests
{
    public interface IReadGuestsRepository
    {
        Task<List<GUESTS?>> GetAllGuestFromMeetingAsync(int meetingId);
        Task<GUESTS?> GetGuestByIdAsync(int guestId);
    }
}
