using DataLibrary.Entities;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.IRepository
{
    public interface IReadRankingsRepository
    {
        Task<List<Ranking>> GetAllRankingsAsync();
        Task<Ranking?> GetRankingByIdAsync(int rankingId, FbTransaction? transaction = null);
    }
}
