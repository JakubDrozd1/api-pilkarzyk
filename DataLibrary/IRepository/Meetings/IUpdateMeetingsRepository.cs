using DataLibrary.Entities;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.IRepository
{
    public interface IUpdateMeetingsRepository
    {
        Task UpdateMeetingAsync(MEETINGS meeting, FbTransaction? transaction = null);
    }
}
