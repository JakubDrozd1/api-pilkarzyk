using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.IRepository
{
    public interface IDeleteMeetingsRepository
    {
        Task DeleteMeetingAsync(int meetingId, FbTransaction? transaction = null);
    }
}
