using System.Data;
using Dapper;
using DataLibrary.Entities;
using DataLibrary.IRepository;
using DataLibrary.Model.DTO.Request;
using DataLibrary.Model.DTO.Response;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository
{
    public class ReadMeetingsRepository(FbConnection dbConnection) : IReadMeetingsRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;
        private static readonly string SELECT
              = $"g.{nameof(GROUPS.NAME)}, " +
                $"u.{nameof(USERS.LOGIN)}, " +
                $"u.{nameof(USERS.PASSWORD)}, " +
                $"u.{nameof(USERS.FIRSTNAME)}, " +
                $"u.{nameof(USERS.SURNAME)}, " +
                $"u.{nameof(USERS.ACCOUNT_TYPE)} AS AccountType, " +
                $"u.{nameof(USERS.EMAIL)}, " +
                $"u.{nameof(USERS.PHONE_NUMBER)} AS PhoneNumber, " +
                $"m.{nameof(MEETINGS.DATE_MEETING)} AS DateMeeting, " +
                $"m.{nameof(MEETINGS.PLACE)}, " +
                $"m.{nameof(MEETINGS.DESCRIPTION)}, " +
                $"m.{nameof(MEETINGS.QUANTITY)} ";
        private static readonly string FROM
              = $"{nameof(MEETINGS)} m " +
                $"LEFT JOIN {nameof(GROUPS)} g ON m.{nameof(MEETINGS.IDGROUP)} = g.{nameof(GROUPS.ID_GROUP)} " +
                $"LEFT JOIN {nameof(USERS)} u ON m.{nameof(MEETINGS.IDUSER)} = u.{nameof(USERS.ID_USER)} ";

        public async Task<List<GetMeetingUsersGroupsResponse>> GetAllMeetingsAsync(GetMeetingsUsersGroupsPaginationRequest getMeetingsRequest, FbTransaction? transaction = null)
        {

            DynamicParameters dynamicParameters = new();
            string WHERE = "1=1";

            if (getMeetingsRequest.IdGroup is not null)
            {
                WHERE += $"AND m.{nameof(MEETINGS.IDGROUP)} = @GroupId ";
                dynamicParameters.Add("@GroupId", getMeetingsRequest.IdGroup);
            }

            if (getMeetingsRequest.IdUser is not null)
            {
                WHERE += $"AND m.{nameof(MEETINGS.IDUSER)} = @UserId ";
                dynamicParameters.Add("@UserId", getMeetingsRequest.IdUser);
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

            var query = new QueryBuilder<MEETINGS>()
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

            return (await db.QueryAsync<GetMeetingUsersGroupsResponse>(query.Build(), dynamicParameters, transaction)).AsList();

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
    }
}
