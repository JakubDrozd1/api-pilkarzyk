using DataLibrary.Entities;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.IRepository.Tokens
{
    public interface IReadTokensRepository
    {
        Task<ACCESS_TOKENS?> GetTokenByUserAsync(int userId, FbTransaction? transaction = null);
    }
}
