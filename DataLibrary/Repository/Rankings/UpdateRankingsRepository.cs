using Dapper;
using DataLibrary.Entities;
using DataLibrary.IRepository;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository
{
    public class UpdateRankingsRepository(FbConnection dbConnection) : IUpdateRankingsRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;

        public async Task UpdateRankingAsync(Ranking ranking)
        {
            var updateBuilder = new QueryBuilder<Ranking>()
               .Update("RANKINGS", ranking)
               .Where("ID_RANKING = @ID_RANKING");
            string updateQuery = updateBuilder.Build();
            using FbConnection db = _dbConnection;
            await db.OpenAsync();
            await db.ExecuteAsync(updateQuery, ranking);
        }
    }
}
