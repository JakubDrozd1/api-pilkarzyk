using DataLibrary.Entities;
using DataLibrary.Model.DTO.Request;
using DataLibrary.Model.DTO.Request.Pagination;
using DataLibrary.Model.DTO.Request.TableRequest;
using DataLibrary.Model.DTO.Response;

namespace BLLLibrary.IService
{
    public interface IMessagesService
    {
        Task<List<GetMessagesUsersMeetingsResponse>> GetAllMessagesAsync(GetMessagesUsersPaginationRequest getMessagesUsersPaginationRequest);
        Task<MESSAGES?> GetMessageByIdAsync(int messageId);
        Task AddMessageAsync(GetMessageRequest getMessageRequest);
        Task UpdateMessageAsync(GetMessageRequest getMessageRequest, int messageId);
        Task UpdateAnswerMessageAsync(GetMessageRequest getMessageRequest);
        Task DeleteMessageAsync(int messageId);
        Task UpdateTeamMessageAsync(GetTeamTableMessageRequest getTeamTableMessageRequest);
        Task SaveChangesAsync();
    }
}
