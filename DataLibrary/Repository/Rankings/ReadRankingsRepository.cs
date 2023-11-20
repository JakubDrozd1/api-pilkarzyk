using FirebirdSql.Data.FirebirdClient;
using DataLibrary.IRepository;
using Dapper;
using DataLibrary.Entities;

namespace DataLibrary.Repository.Rankings
{
    public class ReadRankingsRepository(FbConnection dbConnection) : IReadRankingsRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;

        public async Task<List<Ranking>> GetAllRankingsAsync()
        {
            using FbConnection db = _dbConnection;
            await db.OpenAsync();
            var query = new QueryBuilder<Ranking>()
                .Select("*")
                .From("RANKINGS");
            return (await db.QueryAsync<Ranking>(query.Build())).AsList();
        }

        public async Task<Ranking?> GetRankingByIdAsync(int rankingId)
        {
            using FbConnection db = _dbConnection;
            await db.OpenAsync();
            var query = new QueryBuilder<Ranking>()
                .Select("*")
                .From("RANKINGS")
                .Where("ID_RANKING = @RankingId");
            return await db.QueryFirstOrDefaultAsync<Ranking>(query.Build(), new { RankingId = rankingId });
        }
    }
}
