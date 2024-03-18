using Dapper;
using System.Data;
using DataLibrary.Entities;
using DataLibrary.Helper;
using DataLibrary.IRepository.Teams;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository.Teams
{
    public class UpdateTeamsRepository(FbConnection dbConnection, FbTransaction? fbTransaction) : IUpdateTeamsRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;
        private readonly FbTransaction? _fbTransaction = fbTransaction;
        public async Task UpdateTeamAsync(TEAMS team)
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                await _dbConnection.OpenAsync();
            }
            try
            {
                var updateBuilder = new QueryBuilder<TEAMS>()
                    .Update("TEAMS ", team)
                    .Where("ID_TEAM = @ID_TEAM");
                string updateQuery = updateBuilder.Build();
                await _dbConnection.ExecuteAsync(updateQuery, team, _fbTransaction);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
