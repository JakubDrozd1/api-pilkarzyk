using DataLibrary.IRepository.EmailSender;
using DataLibrary.IRepository.Groups;
using DataLibrary.IRepository.GroupsUsers;
using DataLibrary.IRepository.Meetings;
using DataLibrary.IRepository.Messages;
using DataLibrary.IRepository.Rankings;
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

        IDeleteGroupsRepository DeleteGroupsRepository { get; }
        IDeleteMeetingsRepository DeleteMeetingsRepository { get; }
        IDeleteMessagesRepository DeleteMessagesRepository { get; }
        IDeleteRankingsRepository DeleteRankingsRepository { get; }
        IDeleteUsersRepository DeleteUsersRepository { get; }
        IDeleteGroupsUsersRepository DeleteGroupsUsersRepository { get; }

        IReadGroupsRepository ReadGroupsRepository { get; }
        IReadMeetingsRepository ReadMeetingsRepository { get; }
        IReadMessagesRepository ReadMessagesRepository { get; }
        IReadRankingsRepository ReadRankingsRepository { get; }
        IReadUsersRepository ReadUsersRepository { get; }
        IReadGroupsUsersRepository ReadGroupsUsersRepository { get; }
        IReadEmailSender ReadEmailSender { get; }
        IReadUsersMeetingsRepository ReadUsersMeetingsRepository { get; }

        IUpdateGroupsRepository UpdateGroupsRepository { get; }
        IUpdateMeetingsRepository UpdateMeetingsRepository { get; }
        IUpdateMessagesRepository UpdateMessagesRepository { get; }
        IUpdateRankingsRepository UpdateRankingsRepository { get; }
        IUpdateUsersRepository UpdateUsersRepository { get; }
        IUpdateGroupsUsersRepository UpdateGroupsUsersRepository { get; }

        Task SaveChangesAsync();
    }
}
