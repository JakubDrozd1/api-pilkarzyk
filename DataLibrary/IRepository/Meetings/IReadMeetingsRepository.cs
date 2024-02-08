using DataLibrary.Entities;
using DataLibrary.Model.DTO.Request;
using DataLibrary.Model.DTO.Request.Pagination;
using DataLibrary.Model.DTO.Response;

namespace DataLibrary.IRepository.Meetings
{
    public interface IReadMeetingsRepository
    {
        Task<List<GetMeetingGroupsResponse>> GetAllMeetingsAsync(GetMeetingsGroupsPaginationRequest getMeetingsRequest);
        Task<GetMeetingGroupsResponse?> GetMeetingByIdAsync(int meetingId);
        Task<MEETINGS?> GetMeeting(GetMeetingRequest meetings);
    }
}
