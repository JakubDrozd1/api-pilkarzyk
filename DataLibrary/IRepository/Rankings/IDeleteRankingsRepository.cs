using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.IRepository.Rankings
{
    public interface IDeleteRankingsRepository
    {
        Task DeleteRankingAsync(int rankingId, FbTransaction? transaction = null);
    }
}
