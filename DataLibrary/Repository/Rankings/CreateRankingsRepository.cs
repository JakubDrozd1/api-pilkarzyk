using System.Data;
using Dapper;
using DataLibrary.Entities;
using DataLibrary.Helper;
using DataLibrary.IRepository.Rankings;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository.Rankings
{
    public class CreateRankingsRepository(FbConnection dbConnection, FbTransaction? fbTransaction) : ICreateRankingsRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;
        private readonly FbTransaction? _fbTransaction = fbTransaction;

        public async Task AddRankingAsync(RANKINGS ranking)
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                await _dbConnection.OpenAsync();
            }
            try
            {
                var insertBuilder = new QueryBuilder<RANKINGS>()
                    .Insert("RANKINGS ", ranking);
                string insertQuery = insertBuilder.Build();
                await _dbConnection.ExecuteAsync(insertQuery, ranking, _fbTransaction);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
