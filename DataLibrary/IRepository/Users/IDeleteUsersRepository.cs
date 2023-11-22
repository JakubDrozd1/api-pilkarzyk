using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.IRepository
{
    public interface IDeleteUsersRepository
    {
        Task DeleteUserAsync(int userId, FbTransaction? transaction = null);
    }
}
