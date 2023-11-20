using Dapper;
using DataLibrary.Entities;
using DataLibrary.IRepository.Meetings;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository
{
    public class DeleteMeetingsRepository(FbConnection dbConnection) : IDeleteMeetingsRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;

        public async Task DeleteMeetingAsync(int meetingId)
        {
            var deleteBuilder = new QueryBuilder<Meeting>()
                .Delete("MEETINGS")
                .Where("ID_MEETING = @MeetingId");
            string deleteQuery = deleteBuilder.Build();
            using FbConnection db = _dbConnection;
            await db.OpenAsync();
            await db.ExecuteAsync(deleteQuery, new { MeetingId = meetingId });
        }
    }
}
