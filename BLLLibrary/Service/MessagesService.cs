﻿using BLLLibrary.IService;
using DataLibrary.Entities;
using DataLibrary.Helper.Notification;
using DataLibrary.Model.DTO.Request;
using DataLibrary.Model.DTO.Request.Pagination;
using DataLibrary.Model.DTO.Request.TableRequest;
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
                var author = await _unitOfWork.ReadUsersRepository.GetUserByIdAsync(getMessageRequest.IDUSER ?? throw new Exception("User is null"));
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
            FirebaseNotification notificationHub = new();
            var idUser = user?.ID_USER ?? throw new Exception("User is null");
            var tokens = await _unitOfWork.ReadNotificationTokenRepository.GetAllTokensFromUser(idUser);
            var userDetails = await _unitOfWork.ReadNotificationRepository.GetAllNotificationFromUser(idUser);

            if (tokens != null && userDetails != null)
            {
                if (userDetails.MEETING_ORGANIZER_NOTIFICATION)
                {
                    await notificationHub.SendMessageNotificationAsync(meeting, getMessageRequest, author, tokens);
                }
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

        private async Task SendNotificationToUserTeamAsync(int idUser, int idAuthor, int idMeeting, string? teamName)
        {
            FirebaseNotification notificationHub = new();
            var tokens = await _unitOfWork.ReadNotificationTokenRepository.GetAllTokensFromUser(idUser);
            var userDetails = await _unitOfWork.ReadNotificationRepository.GetAllNotificationFromUser(idUser);
            var author = await _unitOfWork.ReadUsersRepository.GetUserByIdAsync(idAuthor);
            if (tokens != null && userDetails != null)
            {
                if (userDetails.TEAM_NOTIFICATION)
                {
                    await notificationHub.SendNotificationToUserTeamAsync(teamName, idMeeting, author, tokens);
                }
            }
        }

        private async Task SendNotificationToAuthorTeamAsync(int idUser, int idAuthor, int idMeeting, string? teamName)
        {
            FirebaseNotification notificationHub = new();
            var tokens = await _unitOfWork.ReadNotificationTokenRepository.GetAllTokensFromUser(idUser);
            var userDetails = await _unitOfWork.ReadNotificationRepository.GetAllNotificationFromUser(idUser);
            var author = await _unitOfWork.ReadUsersRepository.GetUserByIdAsync(idAuthor);
            if (tokens != null && userDetails != null)
            {
                if (userDetails.TEAM_ORGANIZER_NOTIFICATION)
                {
                    await notificationHub.SendNotificationToAuthorTeamAsync(teamName, idMeeting, author, tokens);
                }
            }
        }
    }
}
