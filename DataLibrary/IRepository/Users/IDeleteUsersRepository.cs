using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.IRepository.Users
{
    public interface IDeleteUsersRepository
    {
        Task DeleteUserAsync(int userId, FbTransaction? transaction = null);
    }
}
