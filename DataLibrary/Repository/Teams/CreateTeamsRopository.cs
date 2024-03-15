using Dapper;
using DataLibrary.Helper;
using System.Data;
using DataLibrary.IRepository.Teams;
using FirebirdSql.Data.FirebirdClient;
using DataLibrary.Model.DTO.Request.TableRequest;

namespace DataLibrary.Repository.Teams
{
    public class CreateTeamsRopository(FbConnection dbConnection, FbTransaction? fbTransaction) : ICreateTeamsRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;
        private readonly FbTransaction? _fbTransaction = fbTransaction;

        public async Task AddTeamsAsync(GetTeamRequest user)
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                await _dbConnection.OpenAsync();
            }
            try
            {
                var insertBuilder = new QueryBuilder<GetTeamRequest>()
                    .Insert("TEAMS ", user);
                string insertQuery = insertBuilder.Build();
                await _dbConnection.ExecuteAsync(insertQuery, user, _fbTransaction);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
