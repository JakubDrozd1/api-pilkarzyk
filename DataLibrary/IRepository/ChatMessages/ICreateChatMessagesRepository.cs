using DataLibrary.Model.DTO.Request.TableRequest;

namespace DataLibrary.IRepository.ChatMessages
{
    public interface ICreateChatMessagesRepository
    {
        Task AddMessageToChat(GetChatMessageRequest getChatMessageRequest);
    }
}
