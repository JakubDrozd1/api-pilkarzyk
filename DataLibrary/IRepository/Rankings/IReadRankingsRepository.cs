using DataLibrary.Entities;

namespace DataLibrary.IRepository
{
    public interface IReadRankingsRepository
    {
        Task<List<Ranking>> GetAllRankingsAsync();
        Task<Ranking?> GetRankingByIdAsync(int rankingId);
    }
}
