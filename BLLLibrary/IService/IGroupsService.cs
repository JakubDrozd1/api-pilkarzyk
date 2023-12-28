using DataLibrary.Entities;
using DataLibrary.Model.DTO.Request;
using DataLibrary.Model.DTO.Request.Pagination;

namespace BLLLibrary.IService
{
    public interface IGroupsService
    {
        Task<List<GROUPS>> GetAllGroupsAsync(GetGroupsPaginationRequest getGroupsPaginationRequest);
        Task<GROUPS?> GetGroupByIdAsync(int groupId);
        Task AddGroupAsync(GetGroupRequest groupRequest);
        Task UpdateGroupAsync(GetGroupRequest groupRequest, int groupId);
        Task DeleteGroupAsync(int groupId);
        Task<GROUPS?> GetGroupByNameAsync(string name);
        Task SaveChangesAsync();
    }
}
