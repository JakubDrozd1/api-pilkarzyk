using DataLibrary.Entities;
using DataLibrary.Model.DTO.Request;
using DataLibrary.Model.DTO.Response;
using WebApi.Model.DTO.Request;

namespace BLLLibrary.IService
{
    public interface IMessagesService
    {
        Task<List<GetMessagesUsersMeetingsResponse>> GetAllMessagesAsync(GetMessagesUsersPaginationRequest getMessagesUsersPaginationRequest);
        Task<MESSAGES?> GetMessageByIdAsync(int messageId);
        Task AddMessageAsync(GetMessageRequest messageRequest);
        Task UpdateMessageAsync(GetMessageRequest messageRequest, int messageId);
        Task DeleteMessageAsync(int messageId);
        Task SaveChangesAsync();
    }
}
