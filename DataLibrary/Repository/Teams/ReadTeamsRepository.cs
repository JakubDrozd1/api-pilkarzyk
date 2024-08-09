using Dapper;
using System.Data;
using DataLibrary.Entities;
using DataLibrary.Helper;
using DataLibrary.IRepository.Teams;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository.Teams
{
    internal class ReadTeamsRepository(FbConnection dbConnection, FbTransaction? fbTransaction) : IReadTeamsRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;
        private readonly FbTransaction? _fbTransaction = fbTransaction;

        public async Task<List<TEAMS?>> GetTeamByMeetingIdAsync(int meetingId)
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                await _dbConnection.OpenAsync();
            }
            try
            {
                var query = new QueryBuilder<TEAMS>()
                    .Select("* ")
                    .From($"{nameof(TEAMS)} ")
                    .Where("IDMEETING = @MeetingId ");
                return (await _dbConnection.QueryAsync<TEAMS?>(query.Build(), new { MeetingId = meetingId }, _fbTransaction)).AsList();
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

        public async Task<TEAMS?> GetTeamByIdAsync(int teamId)
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                await _dbConnection.OpenAsync();
            }
            try
            {
                var query = new QueryBuilder<TEAMS>()
                    .Select("* ")
                    .From($"{nameof(TEAMS)} ")
                    .Where("ID_TEAM = @TeamId ");
                return (await _dbConnection.QueryAsync<TEAMS?>(query.Build(), new { TeamId = teamId }, _fbTransaction)).SingleOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
