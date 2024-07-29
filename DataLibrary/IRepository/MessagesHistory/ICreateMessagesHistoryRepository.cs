using DataLibrary.Model.DTO.Request.TableRequest;

namespace DataLibrary.IRepository.Messages
{
    public interface ICreateMessagesHistoryRepository
    {
        Task AddMessageHistoryAsync(GetMessageHistoryRequest message);
    }
}
