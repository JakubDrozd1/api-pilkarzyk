using DataLibrary.Entities;

namespace BLLLibrary.IService
{
    public interface IGroupsUsersLinkService
    {
        Task<string> GetLinkAsync(int groupId, int userId);
        Task<GROUPS_USERS_LINK?> GetLinkByCodeAsync(string code);
        Task<string> PostLinkAsync(int groupId, int userId);
        Task DeleteLinkAsync(int groupId, int userId);

    }
}
