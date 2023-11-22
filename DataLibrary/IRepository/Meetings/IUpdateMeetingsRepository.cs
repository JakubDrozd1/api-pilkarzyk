using DataLibrary.Entities;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.IRepository
{
    public interface IUpdateMeetingsRepository
    {
        Task UpdateMeetingAsync(Meeting meeting, FbTransaction? transaction = null);
    }
}
