using DataLibrary.Entities;

namespace DataLibrary.IRepository
{
    public interface ICreateRankingsRepository
    { 
        Task AddRankingAsync(Ranking ranking); 
    }
}
