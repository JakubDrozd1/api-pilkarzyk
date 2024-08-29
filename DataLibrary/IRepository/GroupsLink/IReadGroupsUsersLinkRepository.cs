using DataLibrary.Entities;

namespace DataLibrary.IRepository.GroupsLink
{
    public interface IReadGroupsUsersLinkRepository
    {
        Task<GROUPS_USERS_LINK?> ReadGroupLinkAsync(int groupId, int userId);
        Task<GROUPS_USERS_LINK?> ReadLinkByCodeAsync(string code);
    }
}
