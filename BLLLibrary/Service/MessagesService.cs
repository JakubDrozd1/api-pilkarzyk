using BLLLibrary.IService;
using DataLibrary.Entities;
using DataLibrary.UoW;

namespace BLLLibrary.Service
{
    internal class MessagesService(IUnitOfWork unitOfWork) : IMessagesService
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

        public async Task AddMessageAsync(Message message)
        {
            await _unitOfWork.CreateMessagesRepository.AddMessageAsync(message);
        }

        public async Task UpdateMessageAsync(Message message)
        {
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
