using DataLibrary.Entities;
using DataLibrary.Model.DTO.Request;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.IRepository
{
    public interface IReadUsersRepository
    {
        Task<List<USERS>> GetAllUsersAsync(GetUsersPaginationRequest getUsersPaginationRequest, FbTransaction? transaction = null);
        Task<USERS?> GetUserByIdAsync(int userId, FbTransaction? transaction = null);
        Task<USERS?> GetUserByLoginAndPasswordAsync(GetUsersByLoginAndPassword getUsersByLoginAndPassword, FbTransaction? transaction = null);
        Task<USERS?> GetUserByLoginAsync(string? login, FbTransaction? transaction = null);
        Task<List<USERS>> GetAllUsersWithoutGroupAsync(GetUsersWithoutGroupPaginationRequest getUsersWithoutGroupPaginationRequest, FbTransaction? transaction = null);

    }
}
