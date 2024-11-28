using BLLLibrary.IService;
using DataLibrary.Model.DTO.Request.TableRequest;
using DataLibrary.UoW;

namespace BLLLibrary.Service
{
    public class MessagesHistoryService(IUnitOfWork unitOfWork) : IMessagesHistoryService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;


        public async Task AddMessageHistoryAsync(GetMessageHistoryRequest getMessageHistoryRequest)
        {
            await _unitOfWork.CreateMessagesHistoryRepository.AddMessageHistoryAsync(getMessageHistoryRequest);
        }
    }
}
