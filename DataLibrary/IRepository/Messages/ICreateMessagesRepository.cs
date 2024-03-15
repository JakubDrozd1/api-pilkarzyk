using DataLibrary.Model.DTO.Request.TableRequest;

namespace DataLibrary.IRepository.Messages
{
    public interface ICreateMessagesRepository
    {
        Task AddMessageAsync(GetMessageRequest message);
    }
}
