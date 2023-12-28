using DataLibrary.Entities;
using DataLibrary.Model.DTO.Request.Pagination;

namespace DataLibrary.IRepository.Groups
{
    public interface IReadGroupsRepository
    {
        Task<List<GROUPS>> GetAllGroupsAsync(GetGroupsPaginationRequest getGroupsPaginationRequest);
        Task<GROUPS?> GetGroupByIdAsync(int groupId);
        Task<GROUPS?> GetGroupByNameAsync(string name);
    }
}
