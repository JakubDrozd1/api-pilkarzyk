using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.IRepository
{
    public interface IDeleteRankingsRepository
    {
        Task DeleteRankingAsync(int rankingId, FbTransaction? transaction = null);
    }
}
