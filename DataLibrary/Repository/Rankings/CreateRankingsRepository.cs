using System.Data;
using Dapper;
using DataLibrary.Entities;
using DataLibrary.IRepository.Rankings;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository.Rankings
{
    public class CreateRankingsRepository(FbConnection dbConnection) : ICreateRankingsRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;

        public async Task AddRankingAsync(RANKINGS ranking, FbTransaction? transaction = null)
        {
            var insertBuilder = new QueryBuilder<RANKINGS>()
                .Insert("RANKINGS ", ranking);
            string insertQuery = insertBuilder.Build();
            FbConnection db = transaction?.Connection ?? _dbConnection;
            if (transaction == null && db.State != ConnectionState.Open)
            {
                await db.OpenAsync();
            }
            await db.ExecuteAsync(insertQuery, ranking, transaction);
        }
    }
}
