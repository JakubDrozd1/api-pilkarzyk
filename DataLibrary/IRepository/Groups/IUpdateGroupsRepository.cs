using DataLibrary.Entities;

namespace DataLibrary.IRepository
{
    public interface IUpdateGroupsRepository
    {
        Task UpdateGroupAsync(Groupe group);
    }
}
