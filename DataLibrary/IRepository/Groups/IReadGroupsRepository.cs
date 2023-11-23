using DataLibrary.Entities;
using DataLibrary.Model.DTO.Request;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.IRepository
{
    public interface IReadGroupsRepository
    {
        Task<List<GROUPS>> GetAllGroupsAsync(GetGroupsPaginationRequest getGroupsPaginationRequest, FbTransaction? transaction = null);
        Task<GROUPS?> GetGroupByIdAsync(int groupId, FbTransaction? transaction = null);
    }
}
