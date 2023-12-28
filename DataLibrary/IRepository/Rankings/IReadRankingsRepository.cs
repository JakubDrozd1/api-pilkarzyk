using DataLibrary.Entities;
using DataLibrary.Model.DTO.Request.Pagination;
using DataLibrary.Model.DTO.Response;

namespace DataLibrary.IRepository.Rankings
{
    public interface IReadRankingsRepository
    {
        Task<List<GetRankingsUsersGroupsResponse>> GetAllRankingsAsync(GetRankingsUsersGroupsPaginationRequest getRankingsUsersGroupsPaginationRequest);
        Task<RANKINGS?> GetRankingByIdAsync(int rankingId);
    }
}
