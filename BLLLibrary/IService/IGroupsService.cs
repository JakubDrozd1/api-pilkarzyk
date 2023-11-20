using System.Text.RegularExpressions;
using DataLibrary.Entities;

namespace BLLLibrary.IService
{
    internal interface IGroupsService
    {
        Task<List<Groupe>> GetAllGroupsAsync();
        Task<Groupe?> GetGroupByIdAsync(int groupId);
        Task AddGroupAsync(Groupe group);
        Task UpdateGroupAsync(Groupe group);
        Task DeleteGroupAsync(int groupId);
        Task SaveChangesAsync();
    }
}
