using BLLLibrary.IService;
using DataLibrary.Entities;
using DataLibrary.UoW;
using WebApi.Model.DTO.Request;

namespace BLLLibrary.Service
{
    public class RankingsService(IUnitOfWork unitOfWork) : IRankingsService
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

        public async Task AddRankingAsync(RankingRequest rankingRequest)
        {
            Ranking ranking = new()
            {
                DATE_MEETING = rankingRequest.DateMeeting,
                IDGROUP = rankingRequest.IdGroup,
                IDUSER = rankingRequest.IdUser,
                POINT = rankingRequest.Point
            };
            await _unitOfWork.CreateRankingsRepository.AddRankingAsync(ranking);
        }

        public async Task UpdateRankingAsync(RankingRequest rankingRequest, int rankingId)
        {
            Ranking ranking = new()
            {
                ID_RANKING = rankingId,
                DATE_MEETING = rankingRequest.DateMeeting,
                IDGROUP = rankingRequest.IdGroup,
                IDUSER = rankingRequest.IdUser,
                POINT = rankingRequest.Point
            };
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
