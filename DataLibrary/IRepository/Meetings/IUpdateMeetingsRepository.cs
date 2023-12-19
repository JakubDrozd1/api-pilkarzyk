using DataLibrary.Entities;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.IRepository.Meetings
{
    public interface IUpdateMeetingsRepository
    {
        Task UpdateMeetingAsync(MEETINGS meeting, FbTransaction? transaction = null);
    }
}
