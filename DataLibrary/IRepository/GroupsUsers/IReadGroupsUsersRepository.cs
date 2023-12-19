using DataLibrary.Model.DTO.Request;
using DataLibrary.Model.DTO.Response;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.IRepository.GroupsUsers
{
    public interface IReadGroupsUsersRepository
    {
        Task<List<GetGroupsUsersResponse>> GetListGroupsUserAsync(GetUsersGroupsPaginationRequest getUsersGroupsRequest, FbTransaction? transaction = null);
        Task<GetGroupsUsersResponse?> GetUserWithGroup(int groupId, int userId, FbTransaction? transaction = null);
    }
}
