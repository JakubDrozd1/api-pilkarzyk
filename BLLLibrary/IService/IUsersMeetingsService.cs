using DataLibrary.Model.DTO.Request;
using DataLibrary.Model.DTO.Request.Pagination;
using DataLibrary.Model.DTO.Response;

namespace BLLLibrary.IService
{
    public interface IUsersMeetingsService
    {
        Task AddUsersToMeetingAsync(GetUsersMeetingsRequest getUsersMeetingsRequest);
        Task<List<GetMeetingUsersResponse>> GetListMeetingsUsersAsync(GetMeetingsUsersPaginationRequest getMeetingsUsersPaginationRequest);
        Task<GetMeetingUsersResponse?> GetUserWithMeeting(int meetingId, int userId);
        Task SaveChangesAsync();

    }
}
