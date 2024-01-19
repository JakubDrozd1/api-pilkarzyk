using BLLLibrary.IService;
using DataLibrary.Model.DTO.Request;
using DataLibrary.Model.DTO.Request.Pagination;
using DataLibrary.Model.DTO.Response;
using DataLibrary.UoW;

namespace BLLLibrary.Service
{
    public class UsersMeetingsService(IUnitOfWork unitOfWork) : IUsersMeetingsService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task AddUsersToMeetingAsync(GetUsersMeetingsRequest getUsersMeetingsRequest)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                foreach (int userId in getUsersMeetingsRequest.IdUsers)
                {
                    await AddUserToMeetingAsync(getUsersMeetingsRequest.IdMeeting, userId);
                    await _unitOfWork.CreateMessagesRepository.AddMessageAsync(new GetMessageRequest()
                    {
                        IDUSER = userId,
                        IDMEETING = getUsersMeetingsRequest.IdMeeting
                    });
                }
                await _unitOfWork.CreateUsersMeetingRepository.SendNotification(getUsersMeetingsRequest);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollBackTransactionAsync();
                throw new Exception($"{ex.Message}");
            }
        }

        private async Task AddUserToMeetingAsync(int idMeeting, int idUser)
        {
            try
            {
                var meeting = await _unitOfWork.ReadMeetingsRepository.GetMeetingByIdAsync(idMeeting) ?? throw new Exception("Meeting is null");
                var user = await _unitOfWork.ReadUsersRepository.GetUserByIdAsync(idUser) ?? throw new Exception("User is null");
                int meetingId = meeting.ID_MEETING ?? throw new Exception("Meeting is null");
                if (await _unitOfWork.ReadUsersMeetingsRepository.GetUserWithMeeting(meetingId, user.ID_USER) != null)
                {
                    throw new Exception("User is already in this meeting");
                }
                await _unitOfWork.CreateUsersMeetingRepository.AddUserToMeetingAsync(meeting, user);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

        public async Task<List<GetMeetingUsersResponse>> GetListMeetingsUsersAsync(GetMeetingsUsersPaginationRequest getMeetingsUsersPaginationRequest)
        {
            return await _unitOfWork.ReadUsersMeetingsRepository.GetListMeetingsUsersAsync(getMeetingsUsersPaginationRequest);
        }

        public async Task<GetMeetingUsersResponse?> GetUserWithMeeting(int meetingId, int userId)
        {
            return await _unitOfWork.ReadUsersMeetingsRepository.GetUserWithMeeting(meetingId, userId);
        }

        public async Task SaveChangesAsync()
        {
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
