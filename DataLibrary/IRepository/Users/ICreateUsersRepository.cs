using DataLibrary.Entities;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.IRepository
{
    public interface ICreateUsersRepository
    {
        Task AddUserAsync(User user, FbTransaction? transaction = null);
    }
}
