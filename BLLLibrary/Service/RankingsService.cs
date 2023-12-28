using BLLLibrary.IService;
using DataLibrary.Entities;
using DataLibrary.Model.DTO.Request;
using DataLibrary.Model.DTO.Request.Pagination;
using DataLibrary.Model.DTO.Response;
using DataLibrary.UoW;

namespace BLLLibrary.Service
{
    public class RankingsService(IUnitOfWork unitOfWork) : IRankingsService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<List<GetRankingsUsersGroupsResponse>> GetAllRankingsAsync(GetRankingsUsersGroupsPaginationRequest getRankingsUsersGroupsPaginationRequest)
        {
            return await _unitOfWork.ReadRankingsRepository.GetAllRankingsAsync(getRankingsUsersGroupsPaginationRequest);
        }

        public async Task<RANKINGS?> GetRankingByIdAsync(int rankingId)
        {
            return await _unitOfWork.ReadRankingsRepository.GetRankingByIdAsync(rankingId);
        }

        public async Task AddRankingAsync(GetRankingRequest rankingRequest)
        {
            RANKINGS ranking = new()
            {
                DATE_MEETING = rankingRequest.DateMeeting,
                IDGROUP = rankingRequest.IdGroup,
                IDUSER = rankingRequest.IdUser,
                POINT = rankingRequest.Point
            };
            await _unitOfWork.CreateRankingsRepository.AddRankingAsync(ranking);
        }

        public async Task UpdateRankingAsync(GetRankingRequest rankingRequest, int rankingId)
        {
            RANKINGS ranking = new()
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
