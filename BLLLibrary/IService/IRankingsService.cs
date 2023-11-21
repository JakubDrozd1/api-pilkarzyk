using DataLibrary.Entities;
using WebApi.Model.DTO.Request;

namespace BLLLibrary.IService
{
    public interface IRankingsService
    {
        Task<List<Ranking>> GetAllRankingsAsync();
        Task<Ranking?> GetRankingByIdAsync(int rankingId);
        Task AddRankingAsync(RankingRequest rankingRequest);
        Task UpdateRankingAsync(RankingRequest rankingRequest, int rankingId);
        Task DeleteRankingAsync(int rankingId);
        Task SaveChangesAsync();
    }
}
