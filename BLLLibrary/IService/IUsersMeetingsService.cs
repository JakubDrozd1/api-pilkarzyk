using DataLibrary.Model.DTO.Request;
using DataLibrary.Model.DTO.Response;

namespace BLLLibrary.IService
{
    public interface IUsersMeetingsService
    {
        Task AddUserToMeetingAsync(int idMeeting, int idUser);
        Task AddUsersToMeetingAsync(GetUsersMeetingsRequest getUsersMeetingsRequest);
        Task<List<GetMeetingUsersResponse>> GetListMeetingsUsersAsync(GetMeetingsUsersPaginationRequest getMeetingsUsersPaginationRequest);
        Task<GetMeetingUsersResponse?> GetUserWithMeeting(int meetingId, int userId);
        Task SaveChangesAsync();

    }
}
