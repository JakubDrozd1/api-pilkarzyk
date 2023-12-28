using DataLibrary.Model.DTO.Request;

namespace DataLibrary.IRepository.GroupsUsers
{
    public interface ICreateGroupsUsersRepository
    {
        Task AddUserToGroupAsync(GetUserGroupRequest getUserGroupRequest);
    }
}
