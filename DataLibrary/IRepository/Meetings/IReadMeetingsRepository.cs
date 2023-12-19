using DataLibrary.Entities;
using DataLibrary.Model.DTO.Request;
using DataLibrary.Model.DTO.Response;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.IRepository.Meetings
{
    public interface IReadMeetingsRepository
    {
        Task<List<GetMeetingGroupsResponse>> GetAllMeetingsAsync(GetMeetingsGroupsPaginationRequest getMeetingsRequest, FbTransaction? transaction = null);
        Task<MEETINGS?> GetMeetingByIdAsync(int meetingId, FbTransaction? transaction = null);
        Task<MEETINGS?> GetMeeting(GetMeetingRequest meetings, FbTransaction? transaction = null);
    }
}
