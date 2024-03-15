using BLLLibrary.IService;
using DataLibrary.Model.DTO.Request.Pagination;
using DataLibrary.Model.DTO.Request.TableRequest;
using DataLibrary.Model.DTO.Response;
using DataLibrary.UoW;

namespace BLLLibrary.Service
{
    public class ChatMessagesService(IUnitOfWork unitOfWork) : IChatMessagesService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task AddMessageToChat(GetChatMessageRequest getChatMessageRequest)
        {
            await _unitOfWork.CreateChatMessagesRepository.AddMessageToChat(getChatMessageRequest);
        }

        public Task<List<GetChatMessagesResponse>> GetAllChatMessagesFromMeeting(GetChatMessagesPaginationRequest getGroupsPaginationRequest)
        {
            return _unitOfWork.ReadChatMessagesRepository.GetAllChatMessagesFromMeeting(getGroupsPaginationRequest);
        }

        public async Task SaveChangesAsync()
        {
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
