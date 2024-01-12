using DataLibrary.Entities;
using DataLibrary.Model.DTO.Request;

namespace DataLibrary.IRepository.Messages
{
    public interface IUpdateMessagesRepository
    {
        Task UpdateMessageAsync(MESSAGES message);
        Task UpdateAnswerMessageAsync(GetMessageRequest getMessageRequest);
    }
}
