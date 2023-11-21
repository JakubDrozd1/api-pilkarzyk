using DataLibrary.Entities;

namespace DataLibrary.IRepository
{
    public interface IUpdateMessagesRepository
    {
        Task UpdateMessageAsync(Message message);
    }
}
