using DataLibrary.Entities;

namespace BLLLibrary.IService
{
    internal interface IRankingsService
    {
        Task<List<Ranking>> GetAllRankingsAsync();
        Task<Ranking?> GetRankingByIdAsync(int rankingId);
        Task AddRankingAsync(Ranking ranking);
        Task UpdateRankingAsync(Ranking ranking);
        Task DeleteRankingAsync(int rankingId);
        Task SaveChangesAsync();
    }
}
