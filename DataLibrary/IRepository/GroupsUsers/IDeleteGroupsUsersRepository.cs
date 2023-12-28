
namespace DataLibrary.IRepository.GroupsUsers
{
    public interface IDeleteGroupsUsersRepository
    {
        Task DeleteUsersFromGroupAsync(int[] usersId, int groupId);
        Task DeleteAllUsersFromGroupAsync(int groupId);
        Task DeleteAllGroupsFromUser(int userId);
    }
}
