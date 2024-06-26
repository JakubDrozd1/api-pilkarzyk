﻿using DataLibrary.Entities;
using DataLibrary.Model.DTO.Request.TableRequest;
using DataLibrary.Model.DTO.Response;

namespace DataLibrary.Helper.Notification
{
    public interface IFirebaseNotification
    {
        Task SendMeetingNotification(GetMeetingGroupsResponse meeting, List<NOTIFICATION_TOKENS> tokens);
        Task SendGroupNotification(GROUPS group, USERS user, List<NOTIFICATION_TOKENS> tokens);
        Task SendMessageNotificationAsync(GetMeetingGroupsResponse meeting, GetMessageRequest getMessageRequest, USERS? author, List<NOTIFICATION_TOKENS> tokens);
        Task SendNotificationToUserTeamAsync(string? teamName, int? idMeeting, USERS? user, List<NOTIFICATION_TOKENS> tokens);
        Task SendUpdateMeetingNotification(GetMeetingGroupsResponse updated, GetMeetingGroupsResponse meeting, USERS? user, List<NOTIFICATION_TOKENS> tokens);
        Task SendNotificationToAuthorTeamAsync(string? teamName, int? idMeeting, USERS author, List<NOTIFICATION_TOKENS> tokens);
        Task SendGroupAddUserNotification(GROUPS group, List<NOTIFICATION_TOKENS> tokens, USERS author);
        Task SendCancelMeetingNotificationToUser(List<GetMessagesUsersMeetingsResponse> messages, GetMeetingGroupsResponse meeting, List<NOTIFICATION_TOKENS> tokens);
    }
}
