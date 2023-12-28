using DataLibrary.Entities;
using DataLibrary.Model.DTO.Request;
using DataLibrary.Model.DTO.Request.Pagination;
using DataLibrary.Model.DTO.Response;

namespace BLLLibrary.IService
{
    public interface IMeetingsService
    {
        Task<List<GetMeetingGroupsResponse>> GetAllMeetingsAsync(GetMeetingsGroupsPaginationRequest getMeetingsPaginationRequest);
        Task<MEETINGS?> GetMeetingByIdAsync(int meetingId);
        Task AddMeetingAsync(GetMeetingRequest getMeetingRequest);
        Task UpdateMeetingAsync(GetMeetingRequest getMeetingRequest, int meetingId);
        Task DeleteMeetingAsync(int meetingId);
        Task<MEETINGS?> GetMeeting(GetMeetingRequest getMeetingRequest);
        Task SaveChangesAsync();
    }
}
