using DataLibrary.Entities;
using DataLibrary.Model.DTO.Request;
using DataLibrary.Model.DTO.Response;
using WebApi.Model.DTO.Request;

namespace BLLLibrary.IService
{
    public interface IMeetingsService
    {
        Task<List<GetMeetingUsersGroupsResponse>> GetAllMeetingsAsync(GetMeetingsUsersGroupsPaginationRequest getMeetingsPaginationRequest);
        Task<MEETINGS?> GetMeetingByIdAsync(int meetingId);
        Task AddMeetingAsync(GetMeetingRequest meetingRequest);
        Task UpdateMeetingAsync(GetMeetingRequest meetingRequest, int meetingId);
        Task DeleteMeetingAsync(int meetingId);
        Task SaveChangesAsync();
    }
}
