using DataLibrary.Entities;

namespace DataLibrary.IRepository.Groups
{
    public interface ICreateGroupsRepository
    {
        Task AddGroupAsync(Groupe group);
    }
}
