using DataLibrary.Model.DTO.Request.Pagination;
using DataLibrary.Model.DTO.Response;

namespace DataLibrary.IRepository.ChatMessages
{
    public interface IReadChatMessagesRepository
    {
        Task<List<GetChatMessagesResponse>> GetAllChatMessagesFromMeeting(GetChatMessagesPaginationRequest getGroupsPaginationRequest);
    }
}
