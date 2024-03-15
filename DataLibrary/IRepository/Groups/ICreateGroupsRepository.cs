using DataLibrary.Model.DTO.Request.TableRequest;

namespace DataLibrary.IRepository.Groups
{
    public interface ICreateGroupsRepository
    {
        Task AddGroupAsync(GetGroupRequest group);
    }
}
