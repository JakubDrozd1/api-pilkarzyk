using DataLibrary.Entities;

namespace BLLLibrary.IService
{
    internal interface IMeetingsService
    {
        Task<List<Meeting>> GetAllMeetingsAsync();
        Task<Meeting?> GetMeetingByIdAsync(int meetingId);
        Task AddMeetingAsync(Meeting meeting);
        Task UpdateMeetingAsync(Meeting meeting);
        Task DeleteMeetingAsync(int meetingId);
        Task SaveChangesAsync();
    }
}
