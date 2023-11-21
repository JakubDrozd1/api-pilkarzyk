namespace DataLibrary.IRepository
{
    public interface ICreateGroupsUsersRepository
    {
        Task AddUserToGroupAsync(int userId, int groupId);
    }
}
