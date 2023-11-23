using System.Data;
using Dapper;
using DataLibrary.Entities;
using DataLibrary.IRepository;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository
{
    public class DeleteRankingsRepository(FbConnection dbConnection) : IDeleteRankingsRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;

        public async Task DeleteRankingAsync(int rankingId, FbTransaction? transaction = null)
        {
            var deleteBuilder = new QueryBuilder<RANKINGS>()
                .Delete("RANKINGS ")
                .Where("ID_RANKING = @RankingId ");
            string deleteQuery = deleteBuilder.Build();
            FbConnection db = transaction?.Connection ?? _dbConnection;
            if (transaction == null && db.State != ConnectionState.Open)
            {
                await db.OpenAsync();
            }
            await db.ExecuteAsync(deleteQuery, new { RankingId = rankingId }, transaction);
        }
    }
}
