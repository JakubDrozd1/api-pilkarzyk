using DataLibrary.Entities;

namespace DataLibrary.IRepository.Rankings
{
    public interface ICreateRankingsRepository
    {
        Task AddRankingAsync(RANKINGS ranking);
    }
}
