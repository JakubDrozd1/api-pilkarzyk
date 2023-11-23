using BLLLibrary.Model.DTO.Response;
using DataLibrary.Model.DTO.Request;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.IRepository
{
    public interface IReadGroupsUsersRepository
    {
        Task<List<GetGroupsUsersResponse>> GetListGroupsUserAsync(GetUsersGroupsPaginationRequest getUsersGroupsRequest, FbTransaction? transaction = null);
        Task<GetGroupsUsersResponse?> GetUserWithGroup(int groupId, int userId, FbTransaction? transaction = null);
    }
}
