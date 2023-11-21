using DataLibrary.Entities;

namespace DataLibrary.IRepository
{
    public interface ICreateGroupsRepository
    {
        Task AddGroupAsync(Groupe group);
    }
}
