using DataLibrary.Entities;
using DataLibrary.Model.DTO.Request;
using DataLibrary.Model.DTO.Response;
using WebApi.Model.DTO.Request;

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
