using System.Data;
using Dapper;
using DataLibrary.Entities;
using DataLibrary.IRepository;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository
{
    public class ReadMeetingsRepository(FbConnection dbConnection) : IReadMeetingsRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;

        public async Task<List<Meeting>> GetAllMeetingsAsync()
        {
            using FbConnection db = _dbConnection;
            if (db.State != ConnectionState.Open)
                await db.OpenAsync();
            var query = new QueryBuilder<Meeting>()
                .Select("*")
                .From("MEETINGS");
            return (await db.QueryAsync<Meeting>(query.Build())).AsList();
        }

        public async Task<Meeting?> GetMeetingByIdAsync(int meetingId, FbTransaction? transaction = null)
        {
            var query = new QueryBuilder<Meeting>()
                .Select("*")
                .From("MEETINGS")
                .Where("ID_MEETING = @MeetingId");
            FbConnection db = transaction?.Connection ?? _dbConnection;
            if (transaction == null && db.State != ConnectionState.Open)
            {
                await db.OpenAsync();
            }
            return await db.QuerySingleOrDefaultAsync<Meeting>(query.Build(), new { MeetingId = meetingId }, transaction);
        }
    }
}
