using FirebirdSql.Data.FirebirdClient;
using DataLibrary.IRepository;
using Dapper;
using DataLibrary.Entities;

namespace DataLibrary.Repository.Rankings
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
