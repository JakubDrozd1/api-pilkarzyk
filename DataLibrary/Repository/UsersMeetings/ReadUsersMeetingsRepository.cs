using System.Data;
using Dapper;
using DataLibrary.Entities;
using DataLibrary.Helper;
using DataLibrary.IRepository.UsersMeetings;
using DataLibrary.Model.DTO.Request;
using DataLibrary.Model.DTO.Response;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository.UsersMeetings
{
    public class ReadUsersMeetingsRepository(FbConnection dbConnection) : IReadUsersMeetingsRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;
        private static readonly string SELECT
             = $"m.{nameof(MEETINGS.DATE_MEETING)} AS DateMeeting, " +
                $"m.{nameof(MEETINGS.PLACE)}, " +
                $"m.{nameof(MEETINGS.DESCRIPTION)}, " +
                $"m.{nameof(MEETINGS.QUANTITY)}, " +
                $"m.{nameof(MEETINGS.ID_MEETING)} AS IdMeeting, " +
                $"msg.{nameof(MESSAGES.ANSWER)}, " +
                $"u.{nameof(USERS.LOGIN)}, " +
                $"u.{nameof(USERS.ID_USER)} AS IdUser, " +
                $"u.{nameof(USERS.EMAIL)}, " +
                $"u.{nameof(USERS.FIRSTNAME)}, " +
                $"u.{nameof(USERS.SURNAME)}, " +
                $"u.{nameof(USERS.PHONE_NUMBER)} AS PhoneNumber, " +
                $"u.{nameof(USERS.AVATAR)}, " +
                $"u.{nameof(USERS.IS_ADMIN)} AS IsAdmin ";
            private static readonly string FROM
              = $"{nameof(USERS_MEETINGS)} um " +
                $"JOIN {nameof(MEETINGS)} m ON um.{nameof(USERS_MEETINGS.IDMEETING)} = m.{nameof(MEETINGS.ID_MEETING)} " +
                $"JOIN {nameof(MESSAGES)} msg ON um.{nameof(USERS_MEETINGS.IDMEETING)} = msg.{nameof(MESSAGES.IDMEETING)} " +
                $"JOIN {nameof(USERS)} u ON um.{nameof(USERS_MEETINGS.IDUSER)} = u.{nameof(USERS.ID_USER)} ";

        public async Task<List<GetMeetingUsersResponse>> GetListMeetingsUsersAsync(GetMeetingsUsersPaginationRequest getMeetingsUsersPaginationRequest, FbTransaction? transaction = null)
        {
            DynamicParameters dynamicParameters = new();
            string WHERE = "1=1 ";

            if (getMeetingsUsersPaginationRequest.IdMeeting is not null)
            {
                WHERE += $"AND um.{nameof(USERS_MEETINGS.IDMEETING)} = @MeetingId ";
                dynamicParameters.Add("@MeetingId", getMeetingsUsersPaginationRequest.IdMeeting);
            }
            if (getMeetingsUsersPaginationRequest.IdUser is not null)
            {
                WHERE += $"AND um.{nameof(USERS_MEETINGS.IDUSER)} = @UserId ";
                dynamicParameters.Add("@UserId", getMeetingsUsersPaginationRequest.IdUser);
            }
            if (getMeetingsUsersPaginationRequest.DateFrom is not null)
            {
                WHERE += $"AND m.{nameof(MEETINGS.DATE_MEETING)} >= @DateFrom ";
                dynamicParameters.Add("@DateFrom", getMeetingsUsersPaginationRequest.DateFrom);
            }
            if (getMeetingsUsersPaginationRequest.DateTo is not null)
            {
                WHERE += $"AND m.{nameof(MEETINGS.DATE_MEETING)} <= @DateTo ";
                dynamicParameters.Add("@DateTo", getMeetingsUsersPaginationRequest.DateTo);
            }
            if (getMeetingsUsersPaginationRequest.Answer is not null)
            {
                WHERE += $"AND msg.{nameof(MESSAGES.ANSWER)} = @Answer ";
                dynamicParameters.Add("@Answer", getMeetingsUsersPaginationRequest.Answer);
            }

            var query = new QueryBuilder<GetMeetingUsersResponse>()
                    .Select(SELECT)
                    .From(FROM)
                    .Where(WHERE)
                    .OrderBy(getMeetingsUsersPaginationRequest)
                    .Limit(getMeetingsUsersPaginationRequest);

            FbConnection db = transaction?.Connection ?? _dbConnection;
            if (db.State != ConnectionState.Open && transaction == null)
            {
                await db.OpenAsync();
            }
            FbTransaction localTransaction = transaction ?? await db.BeginTransactionAsync();

            return (await db.QueryAsync<GetMeetingUsersResponse>(query.Build(), dynamicParameters, localTransaction)).AsList();
        }

        public async Task<GetMeetingUsersResponse?> GetUserWithMeeting(int meetingId, int userId, FbTransaction? transaction = null)
        {

            var query = new QueryBuilder<GetMeetingUsersResponse>()
                .Select(SELECT)
                .From(FROM)
                .Where("um.IDMEETING = @MeetingId AND um.IDUSER = @UserId AND msg.IDUSER = @UserId");
            FbConnection db = transaction?.Connection ?? _dbConnection;

            if (transaction == null && db.State != ConnectionState.Open)
            {
                await db.OpenAsync();
            }

            return await db.QuerySingleOrDefaultAsync<GetMeetingUsersResponse?>(query.Build(), new { UserId = userId, MeetingId = meetingId }, transaction);

        }
    }
}
