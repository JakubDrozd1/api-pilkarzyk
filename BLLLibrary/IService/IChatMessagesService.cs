using DataLibrary.Model.DTO.Request;
using DataLibrary.Model.DTO.Request.Pagination;
using DataLibrary.Model.DTO.Response;

namespace BLLLibrary.IService
{
    public interface IChatMessagesService
    {
        Task AddMessageToChat(GetChatMessageRequest getChatMessageRequest);
        Task<List<GetChatMessagesResponse>> GetAllChatMessagesFromMeeting(GetChatMessagesPaginationRequest getGroupsPaginationRequest);
        Task SaveChangesAsync();
    }
}
