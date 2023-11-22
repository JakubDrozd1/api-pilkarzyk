using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.IRepository
{
    public interface ICreateGroupsUsersRepository
    {
        Task AddUserToGroupAsync(int userId, int groupId, FbTransaction? transaction = null);
    }
}
