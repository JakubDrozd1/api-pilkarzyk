namespace DataLibrary.IRepository
{
    public interface IDeleteRankingsRepository
    {
        Task DeleteRankingAsync(int rankingId);
    }
}
