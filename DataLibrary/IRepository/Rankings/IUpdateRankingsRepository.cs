using DataLibrary.Entities;

namespace DataLibrary.IRepository.Rankings
{
    public interface IUpdateRankingsRepository
    {
        Task UpdateRankingAsync(RANKINGS ranking);
    }
}
