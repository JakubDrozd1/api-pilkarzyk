using DataLibrary.Entities;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.IRepository.Groups
{
    public interface IUpdateGroupsRepository
    {
        Task UpdateGroupAsync(GROUPS group, FbTransaction? transaction = null);
    }
}
