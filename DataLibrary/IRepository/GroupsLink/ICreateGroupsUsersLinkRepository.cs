namespace DataLibrary.IRepository.GroupsLink
{
    public interface ICreateGroupsUsersLinkRepository
    {
        Task<string> CreateGroupLinkAsync(int groupId, int userId);
    }
}
