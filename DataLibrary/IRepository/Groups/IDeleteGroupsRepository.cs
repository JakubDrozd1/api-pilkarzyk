using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.IRepository
{
    public interface IDeleteGroupsRepository
    {
        Task DeleteGroupAsync(int groupId, FbTransaction? transaction = null);
    }
}
