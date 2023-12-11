using DataLibrary.Model.DTO.Request;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.IRepository
{
    public interface ICreateGroupsUsersRepository
    {
        Task AddUserToGroupAsync(GetUserGroupRequest getUserGroupRequest, FbTransaction? transaction = null);
    }
}
