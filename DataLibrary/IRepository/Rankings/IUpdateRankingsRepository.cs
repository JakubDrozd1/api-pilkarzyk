using DataLibrary.Entities;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.IRepository
{
    public interface IUpdateRankingsRepository
    {
        Task UpdateRankingAsync(Ranking ranking, FbTransaction? transaction = null);
    }
}
