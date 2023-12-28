using DataLibrary.Model.DTO.Request;

namespace DataLibrary.IRepository.Groups
{
    public interface ICreateGroupsRepository
    {
        Task AddGroupAsync(GetGroupRequest group);
    }
}
