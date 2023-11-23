using DataLibrary.Entities;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.IRepository
{
    public interface IUpdateUsersRepository
    {
        Task UpdateUserAsync(USERS user, FbTransaction? transaction = null);
    }
}
