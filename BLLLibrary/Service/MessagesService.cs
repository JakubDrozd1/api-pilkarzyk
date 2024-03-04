using BLLLibrary.IService;
using DataLibrary.Entities;
using DataLibrary.Helper.Notification;
using DataLibrary.Model.DTO.Request;
using DataLibrary.Model.DTO.Request.Pagination;
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

        public async Task AddMessageAsync(GetMessageRequest getMessageRequest)
        {
            await _unitOfWork.CreateMessagesRepository.AddMessageAsync(getMessageRequest);
        }

        public async Task UpdateMessageAsync(GetMessageRequest getMessageRequest, int messageId)
        {
            MESSAGES message = new()
            {
                ID_MESSAGE = messageId,
                IDMEETING = getMessageRequest.IDMEETING,
                IDUSER = getMessageRequest.IDUSER,
                ANSWER = getMessageRequest.ANSWER
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
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _unitOfWork.UpdateMessagesRepository.UpdateAnswerMessageAsync(getMessageRequest);
                var meeting = await _unitOfWork.ReadMeetingsRepository.GetMeetingByIdAsync(getMessageRequest.IDMEETING ?? throw new Exception("Meeting is null"));
                var user = await _unitOfWork.ReadUsersRepository.GetUserByIdAsync(meeting?.IdAuthor ?? throw new Exception("User is null"));
                await _unitOfWork.SaveChangesAsync();
                await SendNotificationToUserAsync(meeting, user, getMessageRequest);

            }
            catch (Exception ex)
            {
                await _unitOfWork.RollBackTransactionAsync();
                throw new Exception($"{ex.Message}");
            }
        }

        private async Task SendNotificationToUserAsync(GetMeetingGroupsResponse meeting, USERS? user, GetMessageRequest getMessageRequest)
        {
            FirebaseNotification notificationHub = new();
            var idUser = user?.ID_USER ?? throw new Exception("User is null");
            var tokens = await _unitOfWork.ReadNotificationTokenRepository.GetAllTokensFromUser(idUser);

            if (tokens != null)
            {
                notificationHub.SendMessageNotificationAsync(meeting, getMessageRequest, tokens);
            }
        }

    }
}
