using DataLibrary.Entities;

namespace DataLibrary.IRepository
{
    public interface IReadMessagesRepository
    {
        Task<List<Message>> GetAllMessagesAsync();
        Task<Message?> GetMessageByIdAsync(int messageId);
    }
}
