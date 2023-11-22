using System.Data;
using Dapper;
using DataLibrary.Entities;
using DataLibrary.IRepository;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository
{
    public class UpdateMeetingsRepository(FbConnection dbConnection) : IUpdateMeetingsRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;

        public async Task UpdateMeetingAsync(Meeting meeting, FbTransaction? transaction = null)
        {
            var updateBuilder = new QueryBuilder<Meeting>()
                .Update("MEETINGS", meeting)
                .Where("ID_MEETING = @ID_MEETING");
            string updateQuery = updateBuilder.Build();
            FbConnection db = transaction?.Connection ?? _dbConnection;
            if (transaction == null && db.State != ConnectionState.Open)
            {
                await db.OpenAsync();
            }
            await db.ExecuteAsync(updateQuery, meeting, transaction);
        }
    }
}
