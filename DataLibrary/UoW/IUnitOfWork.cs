﻿using DataLibrary.IRepository.ChatMessages;
using DataLibrary.IRepository.EmailSender;
using DataLibrary.IRepository.GroupInvite;
using DataLibrary.IRepository.Groups;
using DataLibrary.IRepository.GroupsUsers;
using DataLibrary.IRepository.Guests;
using DataLibrary.IRepository.Meetings;
using DataLibrary.IRepository.Messages;
using DataLibrary.IRepository.Notification;
using DataLibrary.IRepository.NotificationToken;
using DataLibrary.IRepository.Rankings;
using DataLibrary.IRepository.ResetPassword;
using DataLibrary.IRepository.Teams;
using DataLibrary.IRepository.Tokens;
using DataLibrary.IRepository.Users;
using DataLibrary.IRepository.UsersMeetings;

namespace DataLibrary.UoW
{
    public interface IUnitOfWork : IDisposable
    {
        ICreateGroupsRepository CreateGroupsRepository { get; }
        ICreateMeetingsRepository CreateMeetingsRepository { get; }
        ICreateMessagesRepository CreateMessagesRepository { get; }
        ICreateRankingsRepository CreateRankingsRepository { get; }
        ICreateUsersRepository CreateUsersRepository { get; }
        ICreateGroupsUsersRepository CreateGroupsUsersRepository { get; }
        ICreateTokensRepository CreateTokensRepository { get; }
        ICreateUsersMeetingsRepository CreateUsersMeetingRepository { get; }
        ICreateGroupInviteRepository CreateGroupInviteRepository { get; }
        ICreateNotificationTokenRepository CreateNotificationTokenRepository { get; }
        ICreateChatMessagesRepository CreateChatMessagesRepository { get; }
        ICreateResetPasswordRepository CreateResetPasswordRepository { get; }
        ICreateTeamsRepository CreateTeamsRepository { get; }
        ICreateGuestsRepository CreateGuestsRepository { get; }
        ICreateNotificationRepository CreateNotificationRepository { get; }

        IDeleteGroupsRepository DeleteGroupsRepository { get; }
        IDeleteMeetingsRepository DeleteMeetingsRepository { get; }
        IDeleteMessagesRepository DeleteMessagesRepository { get; }
        IDeleteRankingsRepository DeleteRankingsRepository { get; }
        IDeleteUsersRepository DeleteUsersRepository { get; }
        IDeleteGroupsUsersRepository DeleteGroupsUsersRepository { get; }
        IDeleteGroupInviteRepository DeleteGroupInviteRepository { get; }
        IDeleteNotificationTokenRepository DeleteNotificationTokenRepository { get; }
        IDeleteTeamsRepository DeleteTeamsRepository { get; }
        IDeleteGuestsRepository DeleteGuestsRepository { get; }
        IDeleteNotificationRepository DeleteNotificationRepository { get; }

        IReadGroupsRepository ReadGroupsRepository { get; }
        IReadMeetingsRepository ReadMeetingsRepository { get; }
        IReadMessagesRepository ReadMessagesRepository { get; }
        IReadRankingsRepository ReadRankingsRepository { get; }
        IReadUsersRepository ReadUsersRepository { get; }
        IReadGroupsUsersRepository ReadGroupsUsersRepository { get; }
        IReadEmailSender ReadEmailSender { get; }
        IReadUsersMeetingsRepository ReadUsersMeetingsRepository { get; }
        IReadGroupInviteRepository ReadGroupInviteRepository { get; }
        IReadTokensRepository ReadTokensRepository { get; }
        IReadNotificationTokenRepository ReadNotificationTokenRepository { get; }
        IReadChatMessagesRepository ReadChatMessagesRepository { get; }
        IReadResetPasswordRepository ReadResetPasswordRepository { get; }
        IReadTeamsRepository ReadTeamsRepository { get; }
        IReadGuestsRepository ReadGuestsRepository { get; }
        IReadNotificationRepository ReadNotificationRepository { get; }

        IUpdateGroupsRepository UpdateGroupsRepository { get; }
        IUpdateMeetingsRepository UpdateMeetingsRepository { get; }
        IUpdateMessagesRepository UpdateMessagesRepository { get; }
        IUpdateRankingsRepository UpdateRankingsRepository { get; }
        IUpdateUsersRepository UpdateUsersRepository { get; }
        IUpdateGroupsUsersRepository UpdateGroupUsersRepository { get; }
        IUpdateTeamsRepository UpdateTeamsRepository { get; }
        IUpdateGuestsRepository UpdateGuestsRepository { get; }
        IUpdateNotificationRepository UpdateNotificationRepository { get; }

        Task SaveChangesAsync();
        Task BeginTransactionAsync();
        Task RollBackTransactionAsync();
    }
}
