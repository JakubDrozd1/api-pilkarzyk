using DataLibrary.Model.DTO.Request;

namespace DataLibrary.IRepository.Messages
{
    public interface ICreateMessagesRepository
    {
        Task AddMessageAsync(GetMessageRequest message);
    }
}
