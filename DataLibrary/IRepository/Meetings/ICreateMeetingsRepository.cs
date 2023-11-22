using DataLibrary.Entities;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.IRepository
{
    public interface ICreateMeetingsRepository
    {
        Task AddMeetingAsync(Meeting meeting, FbTransaction? transaction = null);
    }
}
