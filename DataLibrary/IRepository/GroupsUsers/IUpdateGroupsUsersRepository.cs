using DataLibrary.Entities;

namespace DataLibrary.IRepository.GroupsUsers
{
    public interface IUpdateGroupsUsersRepository
    {
        Task UpdateGroupUserAsync(GROUPS_USERS groupUsers);
    }
}
