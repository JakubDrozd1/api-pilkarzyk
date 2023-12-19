using DataLibrary.Model.DTO.Request;
using DataLibrary.Model.DTO.Response;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.IRepository.UsersMeetings
{
    public interface IReadUsersMeetingsRepository
    {
        Task<List<GetMeetingUsersResponse>> GetListMeetingsUsersAsync(GetMeetingsUsersPaginationRequest getMeetingsUsersPaginationRequest, FbTransaction? transaction = null);

        Task<GetMeetingUsersResponse?> GetUserWithMeeting(int meetingId, int userId, FbTransaction? transaction = null);

    }
}
