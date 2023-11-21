using DataLibrary.Entities;

namespace DataLibrary.IRepository
{
    public interface IReadMeetingsRepository
    {
        Task<List<Meeting>> GetAllMeetingsAsync();
        Task<Meeting?> GetMeetingByIdAsync(int meetingId);
    }
}
