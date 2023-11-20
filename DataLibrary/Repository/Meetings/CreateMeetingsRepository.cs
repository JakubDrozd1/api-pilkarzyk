using Dapper;
using FirebirdSql.Data.FirebirdClient;
using DataLibrary.IRepository.Meetings;
using DataLibrary.Entities;

namespace DataLibrary.Repository
{
    public class CreateMeetingsRepository(FbConnection dbConnection) : ICreateMeetingsRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;

        public async Task AddMeetingAsync(Meeting meeting)
        {
            var insertBuilder = new QueryBuilder<Meeting>()
                .Insert("MEETINGS", meeting);
            string insertQuery = insertBuilder.Build();
            using FbConnection db = _dbConnection;
            await db.OpenAsync();
            await db.ExecuteAsync(insertQuery, meeting);
        }
    }
}
