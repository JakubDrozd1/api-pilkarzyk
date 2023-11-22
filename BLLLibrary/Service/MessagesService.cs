using BLLLibrary.IService;
using DataLibrary.Entities;
using DataLibrary.UoW;
using WebApi.Model.DTO.Request;

namespace BLLLibrary.Service
{
    public class MessagesService(IUnitOfWork unitOfWork) : IMessagesService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<List<Message>> GetAllMessagesAsync()
        {
            return await _unitOfWork.ReadMessagesRepository.GetAllMessagesAsync();
        }

        public async Task<Message?> GetMessageByIdAsync(int messageId)
        {
            return await _unitOfWork.ReadMessagesRepository.GetMessageByIdAsync(messageId);
        }

        public async Task AddMessageAsync(MessageRequest messageRequest)
        {
            Message message = new()
            {
                IDMEETING = messageRequest.IdMeeting,
                IDUSER = messageRequest.IdUser,
                ANSWER = messageRequest.Answer
            };
            await _unitOfWork.CreateMessagesRepository.AddMessageAsync(message);
        }

        public async Task UpdateMessageAsync(MessageRequest messageRequest, int messageId)
        {
            Message message = new()
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
    }
}
