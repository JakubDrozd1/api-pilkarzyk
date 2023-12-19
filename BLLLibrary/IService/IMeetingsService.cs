using DataLibrary.Entities;
using DataLibrary.Model.DTO.Request;
using DataLibrary.Model.DTO.Response;

namespace BLLLibrary.IService
{
    public interface IMeetingsService
    {
        Task<List<GetMeetingGroupsResponse>> GetAllMeetingsAsync(GetMeetingsGroupsPaginationRequest getMeetingsPaginationRequest);
        Task<MEETINGS?> GetMeetingByIdAsync(int meetingId);
        Task AddMeetingAsync(GetMeetingRequest meetingRequest);
        Task UpdateMeetingAsync(GetMeetingRequest meetingRequest, int meetingId);
        Task DeleteMeetingAsync(int meetingId);
        Task<MEETINGS?> GetMeeting(GetMeetingRequest meeting);
        Task SaveChangesAsync();
    }
}
