using DataLibrary.Entities;
using WebApi.Model.DTO.Request;

namespace BLLLibrary.IService
{
    public interface IMessagesService
    {
        Task<List<Message>> GetAllMessagesAsync();
        Task<Message?> GetMessageByIdAsync(int messageId);
        Task AddMessageAsync(MessageRequest messageRequest);
        Task UpdateMessageAsync(MessageRequest messageRequest, int messageId);
        Task DeleteMessageAsync(int messageId);
        Task SaveChangesAsync();
    }
}
