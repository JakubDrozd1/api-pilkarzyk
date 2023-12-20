using System.Data;
using Dapper;
using DataLibrary.Entities;
using DataLibrary.Helper;
using DataLibrary.IRepository.Rankings;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository.Rankings
{
    public class UpdateRankingsRepository(FbConnection dbConnection) : IUpdateRankingsRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;

        public async Task UpdateRankingAsync(RANKINGS ranking, FbTransaction? transaction = null)
        {
            var updateBuilder = new QueryBuilder<RANKINGS>()
               .Update("RANKINGS ", ranking)
               .Where("ID_RANKING = @ID_RANKING ");
            string updateQuery = updateBuilder.Build();
            FbConnection db = transaction?.Connection ?? _dbConnection;
            if (transaction == null && db.State != ConnectionState.Open)
            {
                await db.OpenAsync();
            }
            await db.ExecuteAsync(updateQuery, ranking, transaction);
        }
    }
}
