using DataLibrary.Entities;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.IRepository
{
    public interface ICreateGroupsRepository
    {
        Task AddGroupAsync(Groupe group, FbTransaction? transaction = null);
    }
}
