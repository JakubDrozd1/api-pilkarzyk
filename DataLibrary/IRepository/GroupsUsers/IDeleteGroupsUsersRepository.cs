using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.IRepository
{
    public interface IDeleteGroupsUsersRepository
    {
        Task DeleteUsersFromGroupAsync(int[] usersId, int groupId, FbTransaction? transaction = null);
        Task DeleteAllUsersFromGroupAsync(int groupId, FbTransaction? transaction = null);
        Task DeleteAllGroupsFromUser(int userId, FbTransaction? transaction = null);
    }
}
