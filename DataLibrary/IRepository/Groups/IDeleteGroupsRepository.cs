using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.IRepository.Groups
{
    public interface IDeleteGroupsRepository
    {
        Task DeleteGroupAsync(int groupId, FbTransaction? transaction = null);
    }
}
