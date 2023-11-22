using DataLibrary.Entities;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.IRepository
{
    public interface IReadGroupsRepository
    {
        Task<List<Groupe>> GetAllGroupsAsync();
        Task<Groupe?> GetGroupByIdAsync(int groupId, FbTransaction? transaction = null);
    }
}
