using BLLLibrary.IService;
using DataLibrary.Entities;
using DataLibrary.Model.DTO.Request;
using DataLibrary.Model.DTO.Response;
using DataLibrary.UoW;

namespace BLLLibrary.Service
{
    public class MessagesService(IUnitOfWork unitOfWork) : IMessagesService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<List<GetMessagesUsersMeetingsResponse>> GetAllMessagesAsync(GetMessagesUsersPaginationRequest getMessagesUsersPaginationRequest)
        {
            return await _unitOfWork.ReadMessagesRepository.GetAllMessagesAsync(getMessagesUsersPaginationRequest);
        }

        public async Task<MESSAGES?> GetMessageByIdAsync(int messageId)
        {
            return await _unitOfWork.ReadMessagesRepository.GetMessageByIdAsync(messageId);
        }

        public async Task AddMessageAsync(GetMessageRequest messageRequest)
        {
            MESSAGES message = new()
            {
                IDMEETING = messageRequest.IdMeeting,
                IDUSER = messageRequest.IdUser,
                ANSWER = messageRequest.Answer
            };
            await _unitOfWork.CreateMessagesRepository.AddMessageAsync(message);
        }

        public async Task UpdateMessageAsync(GetMessageRequest messageRequest, int messageId)
        {
            MESSAGES message = new()
            {
                ID_MESSAGE = messageId,
                IDMEETING = messageRequest.IdMeeting,
                IDUSER = messageRequest.IdUser,
                ANSWER = messageRequest.Answer
            };
            await _unitOfWork.UpdateMessagesRepository.UpdateMessageAsync(message);
        }

        public async Task DeleteMessageAsync(int messageId)
        {
            await _unitOfWork.DeleteMessagesRepository.DeleteMessageAsync(messageId);
        }

        public async Task SaveChangesAsync()
        {
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAnswerMessageAsync(GetMessageRequest getMessageRequest)
        {
            await _unitOfWork.UpdateMessagesRepository.UpdateAnswerMessageAsync(getMessageRequest);
        }
    }
}
