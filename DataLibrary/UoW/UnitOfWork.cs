﻿using System.Data;
using DataLibrary.Helper.ConnectionProvider;
using DataLibrary.Helper.Notification;
using DataLibrary.IRepository.EmailSender;
using DataLibrary.IRepository.GroupInvite;
using DataLibrary.IRepository.Groups;
using DataLibrary.IRepository.GroupsUsers;
using DataLibrary.IRepository.Meetings;
using DataLibrary.IRepository.Messages;
using DataLibrary.IRepository.Rankings;
using DataLibrary.IRepository.Tokens;
using DataLibrary.IRepository.Users;
using DataLibrary.IRepository.UsersMeetings;
using DataLibrary.Repository.EmailSender;
using DataLibrary.Repository.GroupInvite;
using DataLibrary.Repository.Groups;
using DataLibrary.Repository.GroupsUsers;
using DataLibrary.Repository.Meetings;
using DataLibrary.Repository.Messages;
using DataLibrary.Repository.Rankings;
using DataLibrary.Repository.Tokens;
using DataLibrary.Repository.Users;
using DataLibrary.Repository.UsersMeetings;
using FirebirdSql.Data.FirebirdClient;
using Microsoft.AspNetCore.SignalR;

namespace DataLibrary.UoW
{
    public class UnitOfWork(IConnectionProvider _connectionProvider, IHubContext<NotificationHub, INotificationHub> _notificationHub) : IUnitOfWork
    {
        private readonly FbConnection dbConnection = _connectionProvider.GetConnection();
        private FbTransaction? dbTransaction = null;
        readonly IHubContext<NotificationHub, INotificationHub> notificationHub = _notificationHub;

        public ICreateGroupsRepository CreateGroupsRepository => new CreateGroupsRepository(dbConnection, dbTransaction);
        public ICreateMeetingsRepository CreateMeetingsRepository => new CreateMeetingsRepository(dbConnection, dbTransaction);
        public ICreateMessagesRepository CreateMessagesRepository => new CreateMessagesRepository(dbConnection, dbTransaction);
        public ICreateRankingsRepository CreateRankingsRepository => new CreateRankingsRepository(dbConnection, dbTransaction);
        public ICreateUsersRepository CreateUsersRepository => new CreateUsersRepository(dbConnection, dbTransaction);
        public ICreateGroupsUsersRepository CreateGroupsUsersRepository => new CreateGroupsUsersRepository(dbConnection, dbTransaction);
        public ICreateTokensRepository CreateTokensRepository => new CreateTokensRepository(dbConnection, dbTransaction);
        public ICreateUsersMeetingsRepository CreateUsersMeetingRepository => new CreateUsersMeetingsRepository(dbConnection, notificationHub, dbTransaction);
        public ICreateGroupInviteRepository CreateGroupInviteRepository => new CreateGroupInviteRepository(dbConnection, dbTransaction);



        public IDeleteGroupsRepository DeleteGroupsRepository => new DeleteGroupsRepository(dbConnection, dbTransaction);
        public IDeleteMeetingsRepository DeleteMeetingsRepository => new DeleteMeetingsRepository(dbConnection, dbTransaction);
        public IDeleteMessagesRepository DeleteMessagesRepository => new DeleteMessagesRepository(dbConnection, dbTransaction);
        public IDeleteRankingsRepository DeleteRankingsRepository => new DeleteRankingsRepository(dbConnection, dbTransaction);
        public IDeleteUsersRepository DeleteUsersRepository => new DeleteUsersRepository(dbConnection, dbTransaction);
        public IDeleteGroupsUsersRepository DeleteGroupsUsersRepository => new DeleteGroupsUsersRepository(dbConnection, dbTransaction);
        public IDeleteGroupInviteRepository DeleteGroupInviteRepository => new DeleteGroupInviteRepository(dbConnection, dbTransaction);


        public IReadGroupsRepository ReadGroupsRepository => new ReadGroupsRepository(dbConnection, dbTransaction);
        public IReadMeetingsRepository ReadMeetingsRepository => new ReadMeetingsRepository(dbConnection, dbTransaction);
        public IReadMessagesRepository ReadMessagesRepository => new ReadMessagesRepository(dbConnection, dbTransaction);
        public IReadRankingsRepository ReadRankingsRepository => new ReadRankingsRepository(dbConnection, dbTransaction);
        public IReadUsersRepository ReadUsersRepository => new ReadUsersRepository(dbConnection, dbTransaction);
        public IReadGroupsUsersRepository ReadGroupsUsersRepository => new ReadGroupsUsersRepository(dbConnection, dbTransaction);
        public IReadEmailSender ReadEmailSender => new ReadEmailSenderRepository(dbConnection, dbTransaction);
        public IReadUsersMeetingsRepository ReadUsersMeetingsRepository => new ReadUsersMeetingsRepository(dbConnection, dbTransaction);
        public IReadGroupInviteRepository ReadGroupInviteRepository => new ReadGroupInviteRepository(dbConnection, dbTransaction);
        public IReadTokensRepository ReadTokensRepository => new ReadTokensRepository(dbConnection, dbTransaction);



        public IUpdateGroupsRepository UpdateGroupsRepository => new UpdateGroupsRepository(dbConnection, dbTransaction);
        public IUpdateMeetingsRepository UpdateMeetingsRepository => new UpdateMeetingsRepository(dbConnection, dbTransaction);
        public IUpdateMessagesRepository UpdateMessagesRepository => new UpdateMessagesRepository(dbConnection, dbTransaction);
        public IUpdateRankingsRepository UpdateRankingsRepository => new UpdateRankingsRepository(dbConnection, dbTransaction);
        public IUpdateUsersRepository UpdateUsersRepository => new UpdateUsersRepository(dbConnection, dbTransaction);
        public IUpdateGroupsUsersRepository UpdateGroupUsersRepository => new UpdateGroupsUsersRepository(dbConnection, dbTransaction);

        public async Task SaveChangesAsync()
        {
            if (dbTransaction != null)
            {
                await dbTransaction.CommitAsync();
                dbTransaction = null;
            }
        }

        public async void Dispose()
        {
            if (dbConnection != null && dbConnection.State == ConnectionState.Open)
            {
                await dbConnection.CloseAsync();
                await dbConnection.DisposeAsync();
            }
            GC.SuppressFinalize(this);
        }

        public async Task BeginTransactionAsync()
        {
            if (dbConnection.State != ConnectionState.Open)
                await dbConnection.OpenAsync();

            dbTransaction = await dbConnection.BeginTransactionAsync();
        }

        public async Task RollBackTransactionAsync()
        {
            if (dbTransaction != null)
            {
                await dbTransaction.RollbackAsync();
            }
            dbTransaction = null;
        }
    }
}
