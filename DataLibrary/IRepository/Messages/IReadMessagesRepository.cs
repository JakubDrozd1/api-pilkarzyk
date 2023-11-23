using DataLibrary.Entities;
using DataLibrary.Model.DTO.Request;
using DataLibrary.Model.DTO.Response;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.IRepository
{
    public interface IReadMessagesRepository
    {
        Task<List<GetMessagesUsersMeetingsResponse>> GetAllMessagesAsync(GetMessagesUsersPaginationRequest getMeetingsUsersPaginationRequest, FbTransaction? transaction = null);
        Task<MESSAGES?> GetMessageByIdAsync(int messageId, FbTransaction? transaction = null);
    }
}
