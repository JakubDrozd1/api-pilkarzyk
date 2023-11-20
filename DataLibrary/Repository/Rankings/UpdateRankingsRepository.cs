using FirebirdSql.Data.FirebirdClient;
using DataLibrary.IRepository;
using Dapper;
using DataLibrary.Entities;

namespace DataLibrary.Repository.Rankings
{
    public class UpdateRankingsRepository(FbConnection dbConnection) : IUpdateRankingsRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;

        public async Task UpdateRankingAsync(Ranking ranking)
        {
            var updateBuilder = new QueryBuilder<Ranking>()
               .Update("RANKINGS", ranking)
               .Where("ID_RANKING = @RankingId");
            string updateQuery = updateBuilder.Build();
            using FbConnection db = _dbConnection;
            await db.OpenAsync();
            await db.ExecuteAsync(updateQuery, ranking);
        }
    }
}
