using System.Data;
using Dapper;
using DataLibrary.Entities;
using DataLibrary.Helper;
using DataLibrary.IRepository.Meetings;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository.Meetings
{
    public class DeleteMeetingsRepository(FbConnection dbConnection) : IDeleteMeetingsRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;

        public async Task DeleteMeetingAsync(int meetingId, FbTransaction? transaction = null)
        {
            var deleteBuilder = new QueryBuilder<MEETINGS>()
                .Delete("MEETINGS ")
                .Where("ID_MEETING = @MeetingId ");
            string deleteQuery = deleteBuilder.Build();
            FbConnection db = transaction?.Connection ?? _dbConnection;
            if (transaction == null && db.State != ConnectionState.Open)
            {
                await db.OpenAsync();
            }
            await db.ExecuteAsync(deleteQuery, new { MeetingId = meetingId }, transaction);
        }
    }
}
