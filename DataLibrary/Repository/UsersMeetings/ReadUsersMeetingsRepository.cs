using System.Data;
using Dapper;
using DataLibrary.Entities;
using DataLibrary.Helper;
using DataLibrary.IRepository.UsersMeetings;
using DataLibrary.Model.DTO.Request.Pagination;
using DataLibrary.Model.DTO.Response;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository.UsersMeetings
{
    public class ReadUsersMeetingsRepository(FbConnection dbConnection, FbTransaction? fbTransaction) : IReadUsersMeetingsRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;
        private readonly FbTransaction? _fbTransaction = fbTransaction;
        private static readonly string SELECT
             = $"m.{nameof(MEETINGS.DATE_MEETING)} AS DateMeeting, " +
                $"m.{nameof(MEETINGS.PLACE)}, " +
                $"m.{nameof(MEETINGS.DESCRIPTION)}, " +
                $"m.{nameof(MEETINGS.QUANTITY)}, " +
                $"m.{nameof(MEETINGS.ID_MEETING)} AS IdMeeting, " +
                $"msg.{nameof(MESSAGES.ANSWER)}, " +
                $"msg.{nameof(MESSAGES.WAITING_TIME)} AS WaitingTime, " +
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

        public async Task<List<GetMeetingUsersResponse>> GetListMeetingsUsersAsync(GetMeetingsUsersPaginationRequest getMeetingsUsersPaginationRequest)
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                await _dbConnection.OpenAsync();
            }
            try
            {
                DynamicParameters dynamicParameters = new();
                string WHERE = "1=1 ";

                if (getMeetingsUsersPaginationRequest.IdMeeting is not null)
                {
                    WHERE += $"AND um.{nameof(USERS_MEETINGS.IDMEETING)} = @MeetingId ";
                    WHERE += $"AND m.{nameof(MEETINGS.ID_MEETING)} = @MeetingId ";
                    WHERE += $"AND msg.{nameof(MESSAGES.IDMEETING)} = @MeetingId ";
                    dynamicParameters.Add("@MeetingId", getMeetingsUsersPaginationRequest.IdMeeting);
                }
                if (getMeetingsUsersPaginationRequest.IdUser is not null)
                {
                    WHERE += $"AND um.{nameof(USERS_MEETINGS.IDUSER)} = @UserId ";
                    WHERE += $"AND u.{nameof(USERS.ID_USER)} = @UserId ";
                    WHERE += $"AND msg.{nameof(MESSAGES.IDUSER)} = @UserId ";
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
                return (await _dbConnection.QueryAsync<GetMeetingUsersResponse>(query.Build(), dynamicParameters, _fbTransaction)).AsList();
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

        public async Task<GetMeetingUsersResponse?> GetUserWithMeeting(int meetingId, int userId)
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                await _dbConnection.OpenAsync();
            }
            try
            {
                var query = new QueryBuilder<GetMeetingUsersResponse>()
                    .Select(SELECT)
                    .From(FROM)
                    .Where("um.IDMEETING = @MeetingId AND um.IDUSER = @UserId AND msg.IDUSER = @UserId");
                return await _dbConnection.QuerySingleOrDefaultAsync<GetMeetingUsersResponse?>(query.Build(), new { UserId = userId, MeetingId = meetingId }, _fbTransaction);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
