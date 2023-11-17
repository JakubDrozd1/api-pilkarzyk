using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLibrary.IRepository;

namespace DataLibrary.UoW
{
    public interface IUnitOfWork : IDisposable
    {
        IUsersRepository UsersRepository { get; }
        IGroupsRepository GroupsRepository { get; }
        IMeetingsRepository MeetingsRepository { get; }
        IMessagesRepository MessagesRepository { get; }
        IRankingsRepository RankingsRepository { get; }

        Task SaveChangesAsync();
    }
}
