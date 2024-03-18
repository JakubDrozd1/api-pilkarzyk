using Dapper;
using DataLibrary.Entities;
using DataLibrary.Helper;
using System.Data;
using DataLibrary.IRepository.Teams;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository.Teams
{
    public class DeleteTeamsRepository(FbConnection dbConnection, FbTransaction? fbTransaction) : IDeleteTeamsRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;
        private readonly FbTransaction? _fbTransaction = fbTransaction;
        public async Task DeleteTeamAsync(int teamId)
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                await _dbConnection.OpenAsync();
            }
            try
            {
                var deleteBuilder = new QueryBuilder<TEAMS>()
                    .Delete("TEAMS ")
                    .Where("ID_TEAM = @TeamId ");
                string deleteQuery = deleteBuilder.Build();
                await _dbConnection.ExecuteAsync(deleteQuery, new { TeamId = teamId }, _fbTransaction);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
