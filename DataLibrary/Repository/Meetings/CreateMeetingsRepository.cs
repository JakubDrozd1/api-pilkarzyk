using System.Data;
using Dapper;
using DataLibrary.Entities;
using DataLibrary.Helper;
using DataLibrary.IRepository.Meetings;
using DataLibrary.Model.DTO.Request;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository.Meetings
{
    public class CreateMeetingsRepository(FbConnection dbConnection) : ICreateMeetingsRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;

        public async Task AddMeetingAsync(GetMeetingRequest getMeetingRequest, FbTransaction? transaction = null)
        {
            FbConnection db = transaction?.Connection ?? _dbConnection;
            if (transaction == null && db.State != ConnectionState.Open)
            {
                await db.OpenAsync();
            }
            var localTransaction = transaction ?? await db.BeginTransactionAsync();
            try
            {
                ReadMeetingsRepository readMeetingsRepository = new(db);
                MEETINGS meeting = new()
                {
                    DESCRIPTION = getMeetingRequest.Description,
                    QUANTITY = getMeetingRequest.Quantity,
                    DATE_MEETING = getMeetingRequest.DateMeeting,
                    PLACE = getMeetingRequest.Place,
                    IDGROUP = getMeetingRequest.IdGroup,
                };
                var insertBuilder = new QueryBuilder<MEETINGS>()
                    .Insert("MEETINGS ", meeting);
                string insertQuery = insertBuilder.Build();
                var check = await readMeetingsRepository.GetMeeting(getMeetingRequest, localTransaction);
                if (check != null)
                {
                    throw new Exception("Event already exists");
                }

                await db.ExecuteAsync(insertQuery, meeting, localTransaction);

                if (transaction == null)
                {
                    await localTransaction.CommitAsync();
                }
            }
            catch (Exception ex)
            {
                localTransaction?.RollbackAsync();
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
