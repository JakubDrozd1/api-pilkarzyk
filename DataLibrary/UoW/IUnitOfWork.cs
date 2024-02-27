using DataLibrary.IRepository.ChatMessages;
using DataLibrary.IRepository.EmailSender;
using DataLibrary.IRepository.GroupInvite;
using DataLibrary.IRepository.Groups;
using DataLibrary.IRepository.GroupsUsers;
using DataLibrary.IRepository.Meetings;
using DataLibrary.IRepository.Messages;
using DataLibrary.IRepository.NotificationToken;
using DataLibrary.IRepository.Rankings;
using DataLibrary.IRepository.ResetPassword;
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

        IDeleteGroupsRepository DeleteGroupsRepository { get; }
        IDeleteMeetingsRepository DeleteMeetingsRepository { get; }
        IDeleteMessagesRepository DeleteMessagesRepository { get; }
        IDeleteRankingsRepository DeleteRankingsRepository { get; }
        IDeleteUsersRepository DeleteUsersRepository { get; }
        IDeleteGroupsUsersRepository DeleteGroupsUsersRepository { get; }
        IDeleteGroupInviteRepository DeleteGroupInviteRepository { get; }
        IDeleteNotificationTokenRepository DeleteNotificationTokenRepository { get; }

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

        IUpdateGroupsRepository UpdateGroupsRepository { get; }
        IUpdateMeetingsRepository UpdateMeetingsRepository { get; }
        IUpdateMessagesRepository UpdateMessagesRepository { get; }
        IUpdateRankingsRepository UpdateRankingsRepository { get; }
        IUpdateUsersRepository UpdateUsersRepository { get; }
        IUpdateGroupsUsersRepository UpdateGroupUsersRepository { get; }

        Task SaveChangesAsync();
        Task BeginTransactionAsync();
        Task RollBackTransactionAsync();
    }
}
