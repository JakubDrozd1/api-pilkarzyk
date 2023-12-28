using DataLibrary.Model.DTO.Request.Pagination;
using DataLibrary.Model.DTO.Response;

namespace DataLibrary.IRepository.UsersMeetings
{
    public interface IReadUsersMeetingsRepository
    {
        Task<List<GetMeetingUsersResponse>> GetListMeetingsUsersAsync(GetMeetingsUsersPaginationRequest getMeetingsUsersPaginationRequest);

        Task<GetMeetingUsersResponse?> GetUserWithMeeting(int meetingId, int userId);

    }
}
