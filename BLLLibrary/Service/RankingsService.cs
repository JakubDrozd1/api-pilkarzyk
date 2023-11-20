using DataLibrary.Entities;
using DataLibrary.UoW;

namespace BLLLibrary.Service
{
    internal class RankingsService(IUnitOfWork unitOfWork)
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<List<Ranking>> GetAllRankingsAsync()
        {
            return await _unitOfWork.ReadRankingsRepository.GetAllRankingsAsync();
        }

        public async Task<Ranking?> GetRankingByIdAsync(int rankingId)
        {
            return await _unitOfWork.ReadRankingsRepository.GetRankingByIdAsync(rankingId);
        }

        public async Task AddRankingAsync(Ranking ranking)
        {
            await _unitOfWork.CreateRankingsRepository.AddRankingAsync(ranking);
        }

        public async Task UpdateRankingAsync(Ranking ranking)
        {
            await _unitOfWork.UpdateRankingsRepository.UpdateRankingAsync(ranking);
        }

        public async Task DeleteRankingAsync(int rankingId)
        {
            await _unitOfWork.DeleteRankingsRepository.DeleteRankingAsync(rankingId);
        }

        public async Task SaveChangesAsync()
        {
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
