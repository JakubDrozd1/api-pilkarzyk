using DataLibrary.IRepository;
using DataLibrary.IRepository.Groups;
using DataLibrary.IRepository.Meetings;
using DataLibrary.IRepository.Messages;
using DataLibrary.IRepository.Rankings;
using DataLibrary.IRepository.Users;

namespace DataLibrary.UoW
{
    public interface IUnitOfWork : IDisposable
    {
        ICreateGroupsRepository CreateGroupsRepository { get; }
        ICreateMeetingsRepository CreateMeetingsRepository { get; }
        ICreateMessagesRepository CreateMessagesRepository { get; }
        ICreateRankingsRepository CreateRankingsRepository { get; }
        ICreateUsersRepository CreateUsersRepository { get; }

        IDeleteGroupsRepository DeleteGroupsRepository { get; }
        IDeleteMeetingsRepository DeleteMeetingsRepository { get; }
        IDeleteMessagesRepository DeleteMessagesRepository { get; }
        IDeleteRankingsRepository DeleteRankingsRepository { get; }
        IDeleteUsersRepository DeleteUsersRepository { get; }

        IReadGroupsRepository ReadGroupsRepository { get; }
        IReadMeetingsRepository ReadMeetingsRepository { get; }
        IReadMessagesRepository ReadMessagesRepository { get; }
        IReadRankingsRepository ReadRankingsRepository { get; }
        IReadUsersRepository ReadUsersRepository { get; }

        IUpdateGroupsRepository UpdateGroupsRepository { get; }
        IUpdateMeetingsRepository UpdateMeetingsRepository { get; }
        IUpdateMessagesRepository UpdateMessagesRepository { get; }
        IUpdateRankingsRepository UpdateRankingsRepository { get; }
        IUpdateUsersRepository UpdateUsersRepository { get; }

        Task SaveChangesAsync();
    }
}
