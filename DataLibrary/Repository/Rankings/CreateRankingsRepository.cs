using Dapper;
using DataLibrary.Entities;
using DataLibrary.IRepository;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository
{
    public class CreateRankingsRepository(FbConnection dbConnection) : ICreateRankingsRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;

        public async Task AddRankingAsync(Ranking ranking)
        {
            var insertBuilder = new QueryBuilder<Ranking>()
                .Insert("RANKINGS", ranking);
            string insertQuery = insertBuilder.Build();
            using FbConnection db = _dbConnection;
            await db.OpenAsync();
            await db.ExecuteAsync(insertQuery, ranking);
        }
    }
}
