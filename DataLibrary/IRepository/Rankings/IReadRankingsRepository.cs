using DataLibrary.Entities;
using DataLibrary.Model.DTO.Request;
using DataLibrary.Model.DTO.Response;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.IRepository.Rankings
{
    public interface IReadRankingsRepository
    {
        Task<List<GetRankingsUsersGroupsResponse>> GetAllRankingsAsync(GetRankingsUsersGroupsPaginationRequest getRankingsUsersGroupsPaginationRequest, FbTransaction? transaction = null);
        Task<RANKINGS?> GetRankingByIdAsync(int rankingId, FbTransaction? transaction = null);
    }
}
