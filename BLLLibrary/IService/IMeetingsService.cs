using DataLibrary.Entities;
using WebApi.Model.DTO.Request;

namespace BLLLibrary.IService
{
    public interface IMeetingsService
    {
        Task<List<Meeting>> GetAllMeetingsAsync();
        Task<Meeting?> GetMeetingByIdAsync(int meetingId);
        Task AddMeetingAsync(MeetingRequest meetingRequest);
        Task UpdateMeetingAsync(MeetingRequest meetingRequest, int meetingId);
        Task DeleteMeetingAsync(int meetingId);
        Task SaveChangesAsync();
    }
}
