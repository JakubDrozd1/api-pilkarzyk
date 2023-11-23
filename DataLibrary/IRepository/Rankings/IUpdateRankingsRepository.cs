using DataLibrary.Entities;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.IRepository
{
    public interface IUpdateRankingsRepository
    {
        Task UpdateRankingAsync(RANKINGS ranking, FbTransaction? transaction = null);
    }
}
