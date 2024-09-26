namespace DataLibrary.IRepository.GroupsLink
{
    public interface IDeleteGroupsUsersLinkRepository
    {
        Task DeleteGroupLinkAsync(int groupId, int userId);

    }
}
