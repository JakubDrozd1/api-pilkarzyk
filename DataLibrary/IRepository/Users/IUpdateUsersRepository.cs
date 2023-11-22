using DataLibrary.Entities;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.IRepository
{
    public interface IUpdateUsersRepository
    {
        Task UpdateUserAsync(User user, FbTransaction? transaction = null);
    }
}
