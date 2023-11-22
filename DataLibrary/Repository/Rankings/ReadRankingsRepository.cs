using System.Data;
using Dapper;
using DataLibrary.Entities;
using DataLibrary.IRepository;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository
{
    public class ReadRankingsRepository(FbConnection dbConnection) : IReadRankingsRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;

        public async Task<List<Ranking>> GetAllRankingsAsync()
        {
            using FbConnection db = _dbConnection;
            if (db.State != ConnectionState.Open)
                await db.OpenAsync();
            var query = new QueryBuilder<Ranking>()
                .Select("*")
                .From("RANKINGS");
            return (await db.QueryAsync<Ranking>(query.Build())).AsList();
        }

        public async Task<Ranking?> GetRankingByIdAsync(int rankingId, FbTransaction? transaction = null)
        {
            var query = new QueryBuilder<Ranking>()
                .Select("*")
                .From("RANKINGS")
                .Where("ID_RANKING = @RankingId");
            FbConnection db = transaction?.Connection ?? _dbConnection;
            if (transaction == null && db.State != ConnectionState.Open)
            {
                await db.OpenAsync();
            }
            return await db.QuerySingleOrDefaultAsync<Ranking>(query.Build(), new { RankingId = rankingId }, transaction);
        }
    }
}
