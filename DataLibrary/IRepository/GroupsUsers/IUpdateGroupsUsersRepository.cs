namespace DataLibrary.IRepository
{
    public interface IUpdateGroupsUsersRepository
    {
        Task UpdateGroupWithUsersAsync(int[] usersId, int groupId);
        Task UpdateUserWithGroupsAsync(int[] groupsId, int userId);
    }
}
