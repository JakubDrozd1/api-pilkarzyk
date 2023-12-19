using DataLibrary.Entities;
using DataLibrary.Model.DTO.Request;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.IRepository.Meetings
{
    public interface ICreateMeetingsRepository
    {
        Task AddMeetingAsync(GetMeetingRequest getMeetingRequest, FbTransaction? transaction = null);
    }
}
