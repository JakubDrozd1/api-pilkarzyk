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
                var users = await _unitOfWork.ReadGroupsUsersRepository.GetListGroupsUserAsync(new GetUsersGroupsPaginationRequest()
                {
                    Page = 0,
                    OnPage = -1,
                    IdGroup = getUsersMeetingsRequest.Meeting.IDGROUP,
                    IsAvatar = false
                });
                foreach (var user in users)
                {
                    await AddUserToMeetingAsync(meeetingAdded?.ID_MEETING ?? throw new Exception("Meeting is null"), user.IdUser ?? throw new Exception("User is null"));
                    if (user.IdUser != getUsersMeetingsRequest.Message.IDUSER)
                    {
                        await _unitOfWork.CreateMessagesRepository.AddMessageAsync(new GetMessageRequest()
                        {
                            IDUSER = user.IdUser,
                            IDMEETING = meeetingAdded?.ID_MEETING ?? throw new Exception("Meeting is null")
                        });
                    }
                    else
                    {
                        getUsersMeetingsRequest.Message.IDMEETING = meeetingAdded?.ID_MEETING;
                        await _unitOfWork.CreateMessagesRepository.AddMessageAsync(getUsersMeetingsRequest.Message);
                    }
                }
                if (getUsersMeetingsRequest.Team?.Length > 0)
                {
                    foreach (var team in getUsersMeetingsRequest.Team)
                    {
                        team.IDMEETING = meeetingAdded?.ID_MEETING ?? throw new Exception("Meeting is null");
                        await _unitOfWork.CreateTeamsRepository.AddTeamsAsync(team);
                    }
                }
                await _unitOfWork.SaveChangesAsync();
                await SendNotificationToUserAsync(meeetingAdded?.ID_MEETING ?? 0, users, getUsersMeetingsRequest.Message.IDUSER);
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

        private async Task SendNotificationToUserAsync(int idMeeting, List<GetGroupsUsersResponse> users, int? idAuthor)
        {
            FirebaseNotification notificationHub = new();
            var meeting = await _unitOfWork.ReadMeetingsRepository.GetMeetingByIdAsync(idMeeting) ?? throw new Exception("Meeting is null");

            foreach (var user in users)
            {
                if (user.IdUser != idAuthor)
                {
                    var tokens = await _unitOfWork.ReadNotificationTokenRepository.GetAllTokensFromUser(user.IdUser ?? throw new Exception("User is null"));

                    if (tokens != null)
                    {
                        await notificationHub.SendMeetingNotification(meeting, tokens);
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
                IDAUTHOR = getMeetingRequest.IDAUTHOR,
                IS_INDEPENDENT = getMeetingRequest.IS_INDEPENDENT,
            };
            await _unitOfWork.UpdateMeetingsRepository.UpdateMeetingAsync(meeting);
        }

        public async Task UpdateColumnMeetingAsync(GetUpdateMeetingRequest getUpdateMeetingRequest, int meetingId)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var meeting = await _unitOfWork.ReadMeetingsRepository.GetMeetingByIdAsync(meetingId) ?? throw new Exception("Meeting is null");
                var users = await _unitOfWork.ReadGroupsUsersRepository.GetListGroupsUserAsync(new GetUsersGroupsPaginationRequest()
                {
                    IdGroup = meeting.IdGroup,
                    IsAvatar = false,
                    Page = 0,
                    OnPage = -1
                });
                await _unitOfWork.UpdateMeetingsRepository.UpdateColumnMeetingAsync(getUpdateMeetingRequest, meetingId);
                await _unitOfWork.UpdateMessagesRepository.UpdateAnswerMessageAsync(getUpdateMeetingRequest.Message);
                var updated = await _unitOfWork.ReadMeetingsRepository.GetMeetingByIdAsync(meetingId) ?? throw new Exception("Meeting is null");
                await _unitOfWork.SaveChangesAsync();
                if (updated.DateMeeting != meeting.DateMeeting || updated.Place != meeting.Place || updated.Quantity != meeting.Quantity || updated.Description != meeting.Description)
                {
                    await SendUpdateNotificationToUserAsync(updated, meeting, users);
                }
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollBackTransactionAsync();
                throw new Exception($"{ex.Message}");
            }
        }

        private async Task SendUpdateNotificationToUserAsync(GetMeetingGroupsResponse updated, GetMeetingGroupsResponse meeting, List<GetGroupsUsersResponse> users)
        {
            FirebaseNotification notificationHub = new();

            foreach (var user in users)
            {
                if (user.IdUser != meeting.IdAuthor)
                {
                    var tokens = await _unitOfWork.ReadNotificationTokenRepository.GetAllTokensFromUser(user.IdUser ?? throw new Exception("User is null"));

                    if (tokens != null)
                    {
                        await notificationHub.SendUpdateMeetingNotification(updated, meeting, tokens);
                    }
                }
            }
        }

        public async Task DeleteMeetingAsync(int meetingId)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var messages = await _unitOfWork.ReadMessagesRepository.GetAllMessagesAsync(new GetMessagesUsersPaginationRequest()
                {
                    OnPage = -1,
                    Page = 0,
                    DateFrom = DateTime.Now,
                    IdMeeting = meetingId,
                    Answer = "yes",
                });

                var meeting = await _unitOfWork.ReadMeetingsRepository.GetMeetingByIdAsync(meetingId);
                await _unitOfWork.DeleteMeetingsRepository.DeleteMeetingAsync(meetingId);
                await _unitOfWork.SaveChangesAsync();
                await SendCancelMeetingNotificationToUserAsync(messages, meeting ?? throw new Exception("Meetings is null"));

            }
            catch (Exception ex)
            {
                await _unitOfWork.RollBackTransactionAsync();
                throw new Exception($"{ex.Message}");
            }
        }

        private async Task SendCancelMeetingNotificationToUserAsync(List<GetMessagesUsersMeetingsResponse> messages, GetMeetingGroupsResponse meeting)
        {
            FirebaseNotification notificationHub = new();

            foreach (var user in messages)
            {
                if (user.IdUser != meeting.IdAuthor)
                {
                    var tokens = await _unitOfWork.ReadNotificationTokenRepository.GetAllTokensFromUser(user.IdUser ?? throw new Exception("User is null"));

                    if (tokens != null)
                    {
                        await notificationHub.SendCancelMeetingNotificationToUser(messages, meeting, tokens);
                    }
                }
            }
        }

        public async Task SaveChangesAsync()
        {
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
