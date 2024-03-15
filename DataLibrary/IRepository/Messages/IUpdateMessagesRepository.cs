using DataLibrary.Entities;
using DataLibrary.Model.DTO.Request.TableRequest;

namespace DataLibrary.IRepository.Messages
{
    public interface IUpdateMessagesRepository
    {
        Task UpdateMessageAsync(MESSAGES message);
        Task UpdateAnswerMessageAsync(GetMessageRequest getMessageRequest);
        Task UpdateTeamMessageAsync(GetTeamMessageRequest getTeamMessageRequest);

    }
}
