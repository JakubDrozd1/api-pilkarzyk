using DataLibrary.Entities;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.IRepository
{
    public interface IUpdateGroupsRepository
    {
        Task UpdateGroupAsync(GROUPS group, FbTransaction? transaction = null);
    }
}
