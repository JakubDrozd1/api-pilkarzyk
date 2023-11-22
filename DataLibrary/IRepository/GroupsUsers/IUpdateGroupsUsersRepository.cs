using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.IRepository
{
    public interface IUpdateGroupsUsersRepository
    {
        Task UpdateGroupWithUsersAsync(int[] usersId, int groupId, FbTransaction? transaction = null);
        Task UpdateUserWithGroupsAsync(int[] groupsId, int userId, FbTransaction? transaction = null);
    }
}
