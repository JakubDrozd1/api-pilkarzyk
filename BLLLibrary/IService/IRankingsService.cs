using DataLibrary.Entities;
using DataLibrary.Model.DTO.Request;
using DataLibrary.Model.DTO.Request.Pagination;
using DataLibrary.Model.DTO.Response;

namespace BLLLibrary.IService
{
    public interface IRankingsService
    {
        Task<List<GetRankingsUsersGroupsResponse>> GetAllRankingsAsync(GetRankingsUsersGroupsPaginationRequest getRankingsUsersGroupsPaginationRequest);
        Task<RANKINGS?> GetRankingByIdAsync(int rankingId);
        Task AddRankingAsync(GetRankingRequest rankingRequest);
        Task UpdateRankingAsync(GetRankingRequest rankingRequest, int rankingId);
        Task DeleteRankingAsync(int rankingId);
        Task SaveChangesAsync();
    }
}
