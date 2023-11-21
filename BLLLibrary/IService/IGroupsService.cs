using DataLibrary.Entities;
using WebApi.Model.DTO.Request;

namespace BLLLibrary.IService
{
    public interface IGroupsService
    {
        Task<List<Groupe>> GetAllGroupsAsync();
        Task<Groupe?> GetGroupByIdAsync(int groupId);
        Task AddGroupAsync(GroupRequest groupRequest);
        Task UpdateGroupAsync(GroupRequest groupRequest, int groupId);
        Task DeleteGroupAsync(int groupId);
        Task SaveChangesAsync();
    }
}
