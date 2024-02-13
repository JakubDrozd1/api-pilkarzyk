using BLLLibrary.IService;
using DataLibrary.Entities;
using DataLibrary.Helper.Notification;
using DataLibrary.Model.DTO.Request;
using DataLibrary.Model.DTO.Request.Pagination;
using DataLibrary.Model.DTO.Response;
using DataLibrary.UoW;

namespace BLLLibrary.Service
{
    public class MeetingsService(IUnitOfWork unitOfWork) : IMeetingsService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<List<GetMeetingGroupsResponse>> GetAllMeetingsAsync(GetMeetingsGroupsPaginationRequest getMeetingsPaginationRequest)
        {
            return await _unitOfWork.ReadMeetingsRepository.GetAllMeetingsAsync(getMeetingsPaginationRequest);
        }

        public async Task<GetMeetingGroupsResponse?> GetMeetingByIdAsync(int meetingId)
        {
            return await _unitOfWork.ReadMeetingsRepository.GetMeetingByIdAsync(meetingId);
        }

        public async Task<MEETINGS?> GetMeeting(GetMeetingRequest getMeetingRequest)
        {
            return await _unitOfWork.ReadMeetingsRepository.GetMeeting(getMeetingRequest);
        }

        public async Task AddMeetingAsync(GetUsersMeetingsRequest getUsersMeetingsRequest)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var check = await _unitOfWork.ReadMeetingsRepository.GetMeeting(getUsersMeetingsRequest.Meeting);
                if (check != null)
                {
                    throw new Exception("Event already exists");
                }
                await _unitOfWork.CreateMeetingsRepository.AddMeetingAsync(getUsersMeetingsRequest.Meeting);
                var meeetingAdded = await _unitOfWork.ReadMeetingsRepository.GetMeeting(getUsersMeetingsRequest.Meeting);

                foreach (int userId in getUsersMeetingsRequest.IdUsers)
                {
                    await AddUserToMeetingAsync(meeetingAdded?.ID_MEETING ?? 0, userId);
                    if (userId != getUsersMeetingsRequest.Message.IDUSER)
                    {
                        await _unitOfWork.CreateMessagesRepository.AddMessageAsync(new GetMessageRequest()
                        {
                            IDUSER = userId,
                            IDMEETING = meeetingAdded?.ID_MEETING ?? 0
                        });
                    }
                    else
                    {
                        getUsersMeetingsRequest.Message.IDMEETING = meeetingAdded?.ID_MEETING;
                        await _unitOfWork.CreateMessagesRepository.AddMessageAsync(getUsersMeetingsRequest.Message);
                    }
                }
                await _unitOfWork.SaveChangesAsync();
                await SendNotificationToUserAsync(meeetingAdded?.ID_MEETING ?? 0, getUsersMeetingsRequest.IdUsers, getUsersMeetingsRequest.Message.IDUSER);
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
                int meetingId = meeting.IdMeeting ?? throw new Exception("Meeting is null");
                if (await _unitOfWork.ReadUsersMeetingsRepository.GetUserWithMeeting(meetingId, user.ID_USER) != null)
                {
                    throw new Exception("User is already in this meeting");
                }
                await _unitOfWork.CreateUsersMeetingRepository.AddUserToMeetingAsync(meeting, user.ID_USER);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

        private async Task SendNotificationToUserAsync(int idMeeting, int[] idUsers, int? idAuthor)
        {
            FirebaseNotification notificationHub = new();
            var meeting = await _unitOfWork.ReadMeetingsRepository.GetMeetingByIdAsync(idMeeting) ?? throw new Exception("Meeting is null");

            foreach (int userId in idUsers)
            {
                if (userId != idAuthor)
                {
                    var tokens = await _unitOfWork.ReadNotificationTokenRepository.GetAllTokensFromUser(userId);

                    if (tokens != null)
                    {
                        notificationHub.SendMeetingNotification(meeting, tokens);
                    }
                }
            }
        }

        public async Task UpdateMeetingAsync(GetMeetingRequest getMeetingRequest, int meetingId)
        {
            MEETINGS meeting = new()
            {
                ID_MEETING = meetingId,
                DESCRIPTION = getMeetingRequest.DESCRIPTION,
                QUANTITY = getMeetingRequest.QUANTITY,
                DATE_MEETING = getMeetingRequest.DATE_MEETING,
                PLACE = getMeetingRequest.PLACE,
                IDGROUP = getMeetingRequest.IDGROUP,
            };
            await _unitOfWork.UpdateMeetingsRepository.UpdateMeetingAsync(meeting);
        }

        public async Task DeleteMeetingAsync(int meetingId)
        {
            await _unitOfWork.DeleteMeetingsRepository.DeleteMeetingAsync(meetingId);
        }

        public async Task SaveChangesAsync()
        {
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
