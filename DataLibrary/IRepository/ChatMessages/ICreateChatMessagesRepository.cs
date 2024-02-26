using DataLibrary.Model.DTO.Request;

namespace DataLibrary.IRepository.ChatMessages
{
    public interface ICreateChatMessagesRepository
    {
        Task AddMessageToChat(GetChatMessageRequest getChatMessageRequest);
    }
}
