using DataLibrary.Entities;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.IRepository
{
    public interface ICreateTokensRepository
    {
        Task AddTokenAsync(TOKENS tokens, FbTransaction? transaction = null);

    }
}
