using DataLibrary.Entities;
using DataLibrary.Model.DTO.Request;
using DataLibrary.Model.DTO.Request.Pagination;
using DataLibrary.Model.DTO.Response;

namespace BLLLibrary.IService
{
    public interface IMeetingsService
    {
        Task<List<GetMeetingGroupsResponse>> GetAllMeetingsAsync(GetMeetingsGroupsPaginationRequest getMeetingsPaginationRequest);
        Task<GetMeetingGroupsResponse?> GetMeetingByIdAsync(int meetingId);
        Task AddMeetingAsync(GetUsersMeetingsRequest getMeetingRequest);
        Task UpdateMeetingAsync(GetMeetingRequest getMeetingRequest, int meetingId);
        Task DeleteMeetingAsync(int meetingId);
        Task<MEETINGS?> GetMeeting(GetMeetingRequest getMeetingRequest);
        Task SaveChangesAsync();
    }
}
