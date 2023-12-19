using DataLibrary.Entities;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.IRepository.Rankings
{
    public interface IUpdateRankingsRepository
    {
        Task UpdateRankingAsync(RANKINGS ranking, FbTransaction? transaction = null);
    }
}
