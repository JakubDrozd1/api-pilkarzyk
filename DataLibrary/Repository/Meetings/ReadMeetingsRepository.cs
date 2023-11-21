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
            await db.OpenAsync();
            var query = new QueryBuilder<Meeting>()
                .Select("*")
                .From("MEETINGS");
            return (await db.QueryAsync<Meeting>(query.Build())).AsList();
        }

        public async Task<Meeting?> GetMeetingByIdAsync(int meetingId)
        {
            using FbConnection db = _dbConnection;
            await db.OpenAsync();
            var query = new QueryBuilder<Meeting>()
                .Select("*")
                .From("MEETINGS")
                .Where("ID_MEETING = @MeetingId");
            return await db.QueryFirstOrDefaultAsync<Meeting>(query.Build(), new { MeetingId = meetingId });
        }
    }
}
