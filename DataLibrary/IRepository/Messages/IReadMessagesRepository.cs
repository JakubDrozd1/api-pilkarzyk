using DataLibrary.Entities;
using DataLibrary.Model.DTO.Request.Pagination;
using DataLibrary.Model.DTO.Response;

namespace DataLibrary.IRepository.Messages
{
    public interface IReadMessagesRepository
    {
        Task<List<GetMessagesUsersMeetingsResponse>> GetAllMessagesAsync(GetMessagesUsersPaginationRequest getMeetingsUsersPaginationRequest);
        Task<MESSAGES?> GetMessageByIdAsync(int messageId);
    }
}
