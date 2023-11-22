using BLLLibrary.Model.DTO.Response;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.IRepository
{
    public interface IReadGroupsUsersRepository
    {
        Task<List<GetGroupsWithUsersResponse>> GetAllUsersFromGroupAsync(int groupId, FbTransaction? transaction = null);
        Task<List<GetGroupsWithUsersResponse>> GetAllGroupsFromUserAsync(int userId, FbTransaction? transaction = null);
        Task<GetGroupsWithUsersResponse?> GetUserWithGroup(int groupId, int userId, FbTransaction? transaction = null);
    }
}
