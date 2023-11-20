using DataLibrary.Entities;

namespace BLLLibrary.IService
{
    internal interface IMessagesService
    {
        Task<List<Message>> GetAllMessagesAsync();
        Task<Message?> GetMessageByIdAsync(int messageId);
        Task AddMessageAsync(Message message);
        Task UpdateMessageAsync(Message message);
        Task DeleteMessageAsync(int messageId);
        Task SaveChangesAsync();
    }
}
