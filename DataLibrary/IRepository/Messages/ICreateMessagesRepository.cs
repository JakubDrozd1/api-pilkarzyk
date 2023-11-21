using DataLibrary.Entities;

namespace DataLibrary.IRepository
{
    public interface ICreateMessagesRepository
    {
        Task AddMessageAsync(Message message);
    }
}
