using System.Data;
using Dapper;
using DataLibrary.Entities;
using DataLibrary.Helper;
using DataLibrary.IRepository.Meetings;
using DataLibrary.Model.DTO.Request;
using DataLibrary.Model.DTO.Response;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository.Meetings
{
    public class ReadMeetingsRepository(FbConnection dbConnection) : IReadMeetingsRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;
        private static readonly string SELECT
              = $"g.{nameof(GROUPS.NAME)}, " +
                $"m.{nameof(MEETINGS.DATE_MEETING)} AS DateMeeting, " +
                $"m.{nameof(MEETINGS.PLACE)}, " +
                $"m.{nameof(MEETINGS.DESCRIPTION)}, " +
                $"m.{nameof(MEETINGS.QUANTITY)} ";
        private static readonly string FROM
              = $"{nameof(MEETINGS)} m " +
                $"JOIN {nameof(GROUPS)} g ON m.{nameof(MEETINGS.IDGROUP)} = g.{nameof(GROUPS.ID_GROUP)} ";
        public async Task<List<GetMeetingGroupsResponse>> GetAllMeetingsAsync(GetMeetingsGroupsPaginationRequest getMeetingsRequest, FbTransaction? transaction = null)
        {

            DynamicParameters dynamicParameters = new();
            string WHERE = "1=1";

            if (getMeetingsRequest.IdGroup is not null)
            {
                WHERE += $"AND m.{nameof(MEETINGS.IDGROUP)} = @GroupId ";
                dynamicParameters.Add("@GroupId", getMeetingsRequest.IdGroup);
            }

            if (getMeetingsRequest.DateFrom is not null)
            {
                WHERE += $"AND m.{nameof(MEETINGS.DATE_MEETING)} >= @DateFrom ";
                dynamicParameters.Add("@DateFrom", getMeetingsRequest.DateFrom);
            }

            if (getMeetingsRequest.DateTo is not null)
            {
                WHERE += $"AND m.{nameof(MEETINGS.DATE_MEETING)} <= @DateTo ";
                dynamicParameters.Add("@DateTo", getMeetingsRequest.DateTo);
            }

            var query = new QueryBuilder<GetMeetingGroupsResponse>()
                .Select(SELECT)
                .From(FROM)
                .Where(WHERE)
                .OrderBy(getMeetingsRequest)
                .Limit(getMeetingsRequest);
            FbConnection db = transaction?.Connection ?? _dbConnection;

            if (transaction == null && db.State != ConnectionState.Open)
            {
                await db.OpenAsync();
            }

            return (await db.QueryAsync<GetMeetingGroupsResponse>(query.Build(), dynamicParameters, transaction)).AsList();

        }

        public async Task<MEETINGS?> GetMeetingByIdAsync(int meetingId, FbTransaction? transaction = null)
        {

            var query = new QueryBuilder<MEETINGS>()

                .Select("* ")
                .From("MEETINGS ")
                .Where("ID_MEETING = @MeetingId ");
            FbConnection db = transaction?.Connection ?? _dbConnection;

            if (transaction == null && db.State != ConnectionState.Open)
            {
                await db.OpenAsync();
            }

            return await db.QuerySingleOrDefaultAsync<MEETINGS>(query.Build(), new { MeetingId = meetingId }, transaction);

        }

        public async Task<MEETINGS?> GetMeeting(GetMeetingRequest getMeetingRequest, FbTransaction? transaction = null)
        {

            DynamicParameters dynamicParameters = new();
            string WHERE = "1=1 ";

            if (getMeetingRequest.IdGroup is not null)
            {
                WHERE += $"AND {nameof(MEETINGS.IDGROUP)} = @GroupId ";
                dynamicParameters.Add("@GroupId", getMeetingRequest.IdGroup);
            }
            if (getMeetingRequest.Place is not null)
            {
                WHERE += $"AND {nameof(MEETINGS.PLACE)} = @Place ";
                dynamicParameters.Add("@Place", getMeetingRequest.Place);
            }
            if (getMeetingRequest.DateMeeting is not null)
            {
                WHERE += $"AND {nameof(MEETINGS.DATE_MEETING)} = @DateMeeting ";
                dynamicParameters.Add("@DateMeeting", getMeetingRequest.DateMeeting);
            }
            if (getMeetingRequest.Quantity is not null)
            {
                WHERE += $"AND {nameof(MEETINGS.QUANTITY)} = @Quantity ";
                dynamicParameters.Add("@Quantity", getMeetingRequest.Quantity);
            }
            if (getMeetingRequest.Description is not null)
            {
                WHERE += $"AND {nameof(MEETINGS.DESCRIPTION)} = @Description ";
                dynamicParameters.Add("@Description", getMeetingRequest.Description);
            }
            var query = new QueryBuilder<MEETINGS>()
                    .Select("* ")
                    .From("MEETINGS ")
                    .Where(WHERE);

            FbConnection db = transaction?.Connection ?? _dbConnection;

            try
            {
                return await db.QuerySingleOrDefaultAsync<MEETINGS>(query.Build(), dynamicParameters, transaction);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
