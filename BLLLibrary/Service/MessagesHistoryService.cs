using BLLLibrary.IService;
using DataLibrary.Entities;
using DataLibrary.Helper.Notification;
using DataLibrary.Model.DTO.Request;
using DataLibrary.Model.DTO.Request.Pagination;
using DataLibrary.Model.DTO.Request.TableRequest;
using DataLibrary.Model.DTO.Response;
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
