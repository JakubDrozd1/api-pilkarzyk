using System.Data;
using Dapper;
using DataLibrary.Entities;
using DataLibrary.IRepository;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository
{
    public class CreateMeetingsRepository(FbConnection dbConnection) : ICreateMeetingsRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;

        public async Task AddMeetingAsync(MEETINGS meeting, FbTransaction? transaction = null)
        {
            var insertBuilder = new QueryBuilder<MEETINGS>()
                .Insert("MEETINGS ", meeting);
            string insertQuery = insertBuilder.Build();
            FbConnection db = transaction?.Connection ?? _dbConnection;
            if (transaction == null && db.State != ConnectionState.Open)
            {
                await db.OpenAsync();
            }
            await db.ExecuteAsync(insertQuery, meeting, transaction);
        }
    }
}
