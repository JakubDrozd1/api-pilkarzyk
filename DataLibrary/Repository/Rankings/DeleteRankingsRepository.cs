using FirebirdSql.Data.FirebirdClient;
using DataLibrary.IRepository.Rankings;
using Dapper;
using DataLibrary.Entities;

namespace DataLibrary.Repository.Rankings
{
    public class DeleteRankingsRepository(FbConnection dbConnection) : IDeleteRankingsRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;

        public async Task DeleteRankingAsync(int rankingId)
        {
            var deleteBuilder = new QueryBuilder<Ranking>()
                .Delete("RANKINGS")
                .Where("ID_RANKING = @RankingId");
            string deleteQuery = deleteBuilder.Build();
            using FbConnection db = _dbConnection;
            await db.OpenAsync();
            await db.ExecuteAsync(deleteQuery, new { RankingId = rankingId });
        }
    }
}
