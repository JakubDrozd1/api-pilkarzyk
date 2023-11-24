using DataLibrary.Entities;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.IRepository
{
    public interface IReadTokensRepository
    {
        Task<TOKENS?> GetTokenByUserAsync(int userId, FbTransaction? transaction = null);
    }
}
