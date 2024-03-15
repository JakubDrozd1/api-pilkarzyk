using DataLibrary.Model.DTO.Request.TableRequest;

namespace DataLibrary.IRepository.GroupsUsers
{
    public interface ICreateGroupsUsersRepository
    {
        Task AddUserToGroupAsync(GetUserGroupRequest getUserGroupRequest);
    }
}
