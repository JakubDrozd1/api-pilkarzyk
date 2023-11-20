namespace DataLibrary.IRepository.Rankings
{
    public interface IDeleteRankingsRepository
    {
        Task DeleteRankingAsync(int rankingId);
    }
}
