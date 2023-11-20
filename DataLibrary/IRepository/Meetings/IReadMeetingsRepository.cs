using DataLibrary.Entities;

namespace DataLibrary.IRepository.Meetings
{
    public interface IReadMeetingsRepository
    {
        Task<List<Meeting>> GetAllMeetingsAsync();
        Task<Meeting?> GetMeetingByIdAsync(int meetingId);
    }
}
