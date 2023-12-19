using DataLibrary.Entities;
using DataLibrary.Model.DTO.Request;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.IRepository.Users
{
    public interface IUpdateUsersRepository
    {
        Task UpdateUserAsync(USERS user, FbTransaction? transaction = null);
        Task UpdateColumnUserAsync(GetUpdateUserRequest getUpdateUserRequest, int userId, FbTransaction? transaction = null);
    }
}
