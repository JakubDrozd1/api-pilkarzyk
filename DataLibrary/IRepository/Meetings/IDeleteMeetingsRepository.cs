using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.IRepository.Meetings
{
    public interface IDeleteMeetingsRepository
    {
        Task DeleteMeetingAsync(int meetingId);
    }
}
