using Dapper;
using DataLibrary.Entities;
using DataLibrary.IRepository.Meetings;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository
{
    public class UpdateMeetingsRepository(FbConnection dbConnection) : IUpdateMeetingsRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;

        public async Task UpdateMeetingAsync(Meeting meeting)
        {
            var updateBuilder = new QueryBuilder<Meeting>()
                .Update("MEETINGS", meeting)
                .Where("ID_MEETING = @MeetingId");
            string updateQuery = updateBuilder.Build();
            using FbConnection db = _dbConnection;
            await db.OpenAsync();
            await db.ExecuteAsync(updateQuery, meeting);
        }
    }
}
