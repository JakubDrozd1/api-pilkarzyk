using BLLLibrary.IService;
using DataLibrary.Entities;
using DataLibrary.Helper.Notification;
using DataLibrary.Model.DTO.Request;
using DataLibrary.Model.DTO.Request.Pagination;
using DataLibrary.Model.DTO.Request.TableRequest;
using DataLibrary.Model.DTO.Response;
using DataLibrary.UoW;
using Newtonsoft.Json.Linq;

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
                var messageBeforUpdate = await _unitOfWork.ReadMessagesRepository.GetMessageByMeetingIdAndUserIdAsync(
                        getMessageRequest.IDMEETING ?? throw new Exception("Meeting is null"),
                        getMessageRequest.IDUSER ?? throw new Exception("User is null")
                    );
                await _unitOfWork.UpdateMessagesRepository.UpdateAnswerMessageAsync(getMessageRequest);
                var messageAfterUpdate = await _unitOfWork.ReadMessagesRepository.GetMessageByMeetingIdAndUserIdAsync(
                    getMessageRequest.IDMEETING ?? throw new Exception("Meeting is null"),
                    getMessageRequest.IDUSER ?? throw new Exception("User is null")
                );
                var meeting = await _unitOfWork.ReadMeetingsRepository.GetMeetingByIdAsync(getMessageRequest.IDMEETING ?? throw new Exception("Meeting is null"));
                var user = await _unitOfWork.ReadUsersRepository.GetUserByIdAsync(meeting?.IdAuthor ?? throw new Exception("User is null"));
                var author = await _unitOfWork.ReadUsersRepository.GetUserByIdAsync(getMessageRequest.IDUSER ?? throw new Exception("User is null"));

                if (messageBeforUpdate != null && messageAfterUpdate != null)
                {
                    await _unitOfWork.CreateMessagesHistoryRepository.AddMessageHistoryAsync(new GetMessageHistoryRequest()
                    {
                        IDMESSAGE = messageBeforUpdate.ID_MESSAGE,
                        BEFORE_CHANGE = messageBeforUpdate.ANSWER,
                        AFTER_CHANGE = messageAfterUpdate.ANSWER,
                        DATE_CHANGE =  DateTime.Now,
                    });
                }
                await _unitOfWork.SaveChangesAsync();
                await SendNotificationToUserAsync(meeting, user, author, getMessageRequest);

            }
            catch (Exception ex)
            {
                await _unitOfWork.RollBackTransactionAsync();
                throw new Exception($"{ex.Message}");
            }
        }

        private async Task SendNotificationToUserAsync(GetMeetingGroupsResponse meeting, USERS? user, USERS? author, GetMessageRequest getMessageRequest)
        {
            FirebaseNotification notificationHub = new(_unitOfWork);
            var idUser = user?.ID_USER ?? throw new Exception("User is null");
            var tokens = await _unitOfWork.ReadNotificationTokenRepository.GetAllTokensFromUser(idUser);
            var userDetails = await _unitOfWork.ReadNotificationRepository.GetAllNotificationFromUser(idUser);

            if (tokens != null && userDetails != null)
            {
                if (userDetails.MEETING_ORGANIZER_NOTIFICATION)
                {
                    await notificationHub.SendMessageNotificationAsync(meeting, getMessageRequest, author, tokens);
                }
                string title = "";
                switch (getMessageRequest.ANSWER)
                {
                    case "yes":
                        {
                            title = author?.FIRSTNAME + " " + author?.SURNAME + " właśnie zaakceptował twoje zaproszenie do spotkania!";
                        }
                        break;
                    case "no":
                        {
                            title = author?.FIRSTNAME + " " + author?.SURNAME + " właśnie odrzucił twoje zaproszenie do spotkania!";
                        }
                        break;
                }

                await _unitOfWork.CreateNotificationRepository.AddNotificationMessageToUserAsync(
                    new GetNotificationMessageRequest
                    {
                        IDUSER = userDetails.IDUSER,
                        IDGROUP = meeting.IdGroup,
                        IDMEETING = meeting.IdMeeting,
                        DATE_SEND = DateTime.Now,
                        TITLE = title,
                        MESSAGE = meeting.DateMeeting?.ToString("dd-MM-yyyy HH:mm") + " " + meeting.Place + " " + meeting.Description,
                    }
                );
            }
        }

        public async Task UpdateTeamMessageAsync(GetTeamTableMessageRequest getTeamTableMessageRequest)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                if (getTeamTableMessageRequest.Teams != null)
                {
                    foreach (var team in getTeamTableMessageRequest.Teams)
                    {
                        if (getTeamTableMessageRequest.UpdatedTeams != null)
                        {
                            if (getTeamTableMessageRequest.UpdatedTeams.TryGetValue(team.ID_TEAM ?? 0, out List<GetMessagesUsersMeetingsResponse>? addTeams))
                            {
                                var updated = getTeamTableMessageRequest.UpdatedTeams[team.ID_TEAM ?? 0];
                                foreach (var user in updated)

                                    if (user.IdTeam != team.ID_TEAM)
                                    {
                                        if (user.IdUser != null)
                                        {
                                            await _unitOfWork.UpdateMessagesRepository.UpdateTeamMessageAsync(new GetTeamMessageRequest()
                                            {
                                                IDMEETING = team.IDMEETING,
                                                IDTEAM = team.ID_TEAM,
                                                IDUSER = user.IdUser
                                            });
                                        }
                                        else
                                        {
                                            await _unitOfWork.UpdateGuestsRepository.UpdateGuestsAsync(new GUESTS()
                                            {
                                                IDMEETING = team.IDMEETING ?? throw new Exception("Meeting is null"),
                                                IDTEAM = team.ID_TEAM,
                                                ID_GUEST = user.IdGuest ?? throw new Exception("Guest is null"),
                                                NAME = user.Firstname
                                            });
                                        }
                                    }
                            }
                            if (getTeamTableMessageRequest.UpdatedTeams.TryGetValue(0, out List<GetMessagesUsersMeetingsResponse>? removeTeams))
                            {
                                var removeUser = getTeamTableMessageRequest.UpdatedTeams[0];
                                if (removeUser.Count > 0)
                                {
                                    foreach (var user in removeUser)
                                    {
                                        if (user.IdTeam != null)
                                        {
                                            if (user.IdUser != null)
                                            {
                                                await _unitOfWork.UpdateMessagesRepository.UpdateTeamMessageAsync(new GetTeamMessageRequest()
                                                {
                                                    IDMEETING = team.IDMEETING,
                                                    IDTEAM = null,
                                                    IDUSER = user.IdUser
                                                });
                                            }
                                            else
                                            {
                                                await _unitOfWork.UpdateGuestsRepository.UpdateGuestsAsync(new GUESTS()
                                                {
                                                    IDMEETING = team.IDMEETING ?? throw new Exception("Meeting is null"),
                                                    IDTEAM = null,
                                                    ID_GUEST = user.IdGuest ?? throw new Exception("Guest is null"),
                                                    NAME = user.Firstname
                                                });
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    var meeting = await _unitOfWork.ReadMeetingsRepository.GetMeetingByIdAsync(getTeamTableMessageRequest.Teams[0].IDMEETING ?? throw new Exception("Meeting is null"));
                    await _unitOfWork.SaveChangesAsync();


                    foreach (var team in getTeamTableMessageRequest.Teams)
                    {
                        if (getTeamTableMessageRequest.UpdatedTeams != null)
                        {
                            if (getTeamTableMessageRequest.UpdatedTeams.TryGetValue(team.ID_TEAM ?? 0, out List<GetMessagesUsersMeetingsResponse>? addTeams))
                            {
                                var updated = getTeamTableMessageRequest.UpdatedTeams[team.ID_TEAM ?? 0];
                                foreach (var user in updated)
                                    if (user.IdTeam != team.ID_TEAM)
                                    {
                                        if (user.IdUser != null)
                                        {
                                            if (user.IdUser != user.IdAuthor)
                                            {
                                                await SendNotificationToUserTeamAsync(user.IdUser ?? throw new Exception("User is null"), user.IdAuthor ?? throw new Exception("User is null"), meeting?.IdMeeting ?? throw new Exception("Meeting is null"), team.NAME);
                                            }
                                            if (meeting?.IdAuthor != user.IdAuthor)
                                            {
                                                await SendNotificationToAuthorTeamAsync(meeting?.IdAuthor ?? throw new Exception("User is null"), user.IdUser ?? throw new Exception("User is null"), meeting?.IdMeeting ?? throw new Exception("Meeting is null"), team.NAME);
                                            }
                                        }
                                    }
                            }
                        }
                    }
                    if (getTeamTableMessageRequest.UpdatedTeams != null)
                        if (getTeamTableMessageRequest.UpdatedTeams.TryGetValue(0, out List<GetMessagesUsersMeetingsResponse>? removeTeams))
                        {
                            var removeTeam = getTeamTableMessageRequest.UpdatedTeams?[0];
                            if (removeTeam?.Count > 0)
                            {
                                foreach (var user in removeTeam)
                                {
                                    if (user.IdTeam != null)
                                    {
                                        if (user.IdUser != null)
                                        {
                                            if (user.IdUser != user.IdAuthor)
                                            {
                                                await SendNotificationToUserTeamAsync(user.IdUser ?? throw new Exception("User is null"), meeting?.IdAuthor ?? throw new Exception("User is null"), meeting?.IdMeeting ?? throw new Exception("Meeting is null"), null);
                                            }
                                            if (meeting?.IdAuthor != user.IdAuthor)
                                            {
                                                await SendNotificationToAuthorTeamAsync(meeting?.IdAuthor ?? throw new Exception("User is null"), user.IdUser ?? throw new Exception("User is null"), meeting?.IdMeeting ?? throw new Exception("Meeting is null"), null);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                }

            }
            catch (Exception ex)
            {
                await _unitOfWork.RollBackTransactionAsync();
                throw new Exception($"{ex.Message}");
            }
        }

        public async Task UpdateTeamMessageOneAsync(GetTeamTableMessageOneRequest getTeamTableMessageOneRequest)
        {

            if (getTeamTableMessageOneRequest.IdUser != null)
            {
                await _unitOfWork.UpdateMessagesRepository.UpdateTeamMessageAsync(new GetTeamMessageRequest()
                {
                    IDMEETING = getTeamTableMessageOneRequest.IdMeeting,
                    IDTEAM = getTeamTableMessageOneRequest.IdTeam,
                    IDUSER = getTeamTableMessageOneRequest.IdUser
                });

            } else
            {
                    var guest = await _unitOfWork.ReadGuestsRepository.GetGuestByIdAsync(getTeamTableMessageOneRequest.IdGuest ?? throw new Exception("Guest is null"));
                    await _unitOfWork.UpdateGuestsRepository.UpdateGuestsAsync(new GUESTS()
                    {
                        IDMEETING = getTeamTableMessageOneRequest.IdMeeting,
                        IDTEAM = getTeamTableMessageOneRequest.IdTeam,
                        ID_GUEST = getTeamTableMessageOneRequest.IdGuest ?? throw new Exception("Guest is null"),
                        NAME = guest?.NAME ?? throw new Exception("Guest is null")
,
                    });


            }

            var meeting = await _unitOfWork.ReadMeetingsRepository.GetMeetingByIdAsync(getTeamTableMessageOneRequest.IdMeeting);
            var team = await _unitOfWork.ReadTeamsRepository.GetTeamByIdAsync(getTeamTableMessageOneRequest.IdMeeting);


            if(getTeamTableMessageOneRequest.IdUser != null)
            {
                if (getTeamTableMessageOneRequest.IdUser != getTeamTableMessageOneRequest.IdAuthor)
                {
                    await SendNotificationToUserTeamAsync(getTeamTableMessageOneRequest.IdUser ?? throw new Exception("User is null"), meeting?.IdAuthor ?? throw new Exception("User is null"), meeting?.IdMeeting ?? throw new Exception("Meeting is null"), team?.NAME);
                }
                if (meeting?.IdAuthor != getTeamTableMessageOneRequest.IdAuthor)
                {
                    await SendNotificationToAuthorTeamAsync(meeting?.IdAuthor ?? throw new Exception("User is null"), getTeamTableMessageOneRequest.IdUser ?? throw new Exception("User is null"), meeting?.IdMeeting ?? throw new Exception("Meeting is null"), team?.NAME);
                }
            }
        }


        private async Task SendNotificationToUserTeamAsync(int idUser, int idAuthor, int idMeeting, string? teamName)
        {
            FirebaseNotification notificationHub = new(_unitOfWork);
            var tokens = await _unitOfWork.ReadNotificationTokenRepository.GetAllTokensFromUser(idUser);
            var userDetails = await _unitOfWork.ReadNotificationRepository.GetAllNotificationFromUser(idUser);
            var author = await _unitOfWork.ReadUsersRepository.GetUserByIdAsync(idAuthor);
            if (tokens != null && userDetails != null)
            {
                if (userDetails.TEAM_NOTIFICATION)
                {
                    await notificationHub.SendNotificationToUserTeamAsync(teamName, idMeeting, author, tokens);
                }

                string body;
                string title;
                if (teamName != null)
                {
                    title = "Zostałeś dodany do drużyny!";
                    body = author?.FIRSTNAME + " " + author?.SURNAME + " właśnie dodał Cię do drużyny " + teamName;
                }
                else
                {
                    title = "Zostałeś usuniety z drużyny!";
                    body = author?.FIRSTNAME + " " + author?.SURNAME + " właśnie usunął Cię z drużyny i przeniósł do rezerwy";
                }
                await _unitOfWork.CreateNotificationRepository.AddNotificationMessageToUserAsync(
                    new GetNotificationMessageRequest
                    {
                        IDUSER = idUser,
                        IDMEETING = idMeeting,
                        DATE_SEND = DateTime.Now,
                        TITLE = title,
                        MESSAGE = body,
                    }
                );
            }
        }

        private async Task SendNotificationToAuthorTeamAsync(int idUser, int idAuthor, int idMeeting, string? teamName)
        {
            FirebaseNotification notificationHub = new(_unitOfWork);
            var tokens = await _unitOfWork.ReadNotificationTokenRepository.GetAllTokensFromUser(idUser);
            var userDetails = await _unitOfWork.ReadNotificationRepository.GetAllNotificationFromUser(idUser);
            var author = await _unitOfWork.ReadUsersRepository.GetUserByIdAsync(idAuthor);
            if (tokens != null && userDetails != null)
            {
                if (userDetails.TEAM_ORGANIZER_NOTIFICATION)
                {
                    await notificationHub.SendNotificationToAuthorTeamAsync(teamName, idMeeting, author, tokens);
                }
                string title;
                string body;
                if (teamName != null)
                {
                    title = "Ktoś właśnie dołączył do drużyny!";
                    body = author?.FIRSTNAME + " " + author?.SURNAME + " właśnie dołączył do drużyny " + teamName;
                }
                else
                {
                    title = "Ktoś właśnie opuścił drużynę!";
                    body = author?.FIRSTNAME + " " + author?.SURNAME + " opuścił drużynę i przeszedł do rezerwy";
                }

                await _unitOfWork.CreateNotificationRepository.AddNotificationMessageToUserAsync(
                    new GetNotificationMessageRequest
                    {
                        IDUSER = idAuthor,
                        IDMEETING = idMeeting,
                        DATE_SEND = DateTime.Now,
                        MESSAGE = body,
                        TITLE = title,
                    }
                );
            }
        }
    }
}
