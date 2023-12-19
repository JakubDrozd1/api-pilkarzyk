using DataLibrary.Model.DTO.Request;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.IRepository.GroupsUsers
{
    public interface ICreateGroupsUsersRepository
    {
        Task AddUserToGroupAsync(GetUserGroupRequest getUserGroupRequest, FbTransaction? transaction = null);
    }
}
