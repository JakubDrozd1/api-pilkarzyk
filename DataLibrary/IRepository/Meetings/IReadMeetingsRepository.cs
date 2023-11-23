using DataLibrary.Entities;
using DataLibrary.Model.DTO.Request;
using DataLibrary.Model.DTO.Response;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.IRepository
{
    public interface IReadMeetingsRepository
    {
        Task<List<GetMeetingUsersGroupsResponse>> GetAllMeetingsAsync(GetMeetingsUsersGroupsPaginationRequest getMeetingsRequest, FbTransaction? transaction = null);
        Task<MEETINGS?> GetMeetingByIdAsync(int meetingId, FbTransaction? transaction = null);
    }
}
