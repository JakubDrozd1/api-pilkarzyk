using DataLibrary.Entities;

namespace DataLibrary.IRepository
{
    public interface IUpdateRankingsRepository
    {
        Task UpdateRankingAsync(Ranking ranking);
    }
}
