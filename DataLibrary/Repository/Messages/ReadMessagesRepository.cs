using System.Data;
using Dapper;
using DataLibrary.Entities;
using DataLibrary.Helper;
using DataLibrary.IRepository.Messages;
using DataLibrary.Model.DTO.Request;
using DataLibrary.Model.DTO.Response;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository.Messages
{
    public class ReadMessagesRepository(FbConnection dbConnection) : IReadMessagesRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;
        private static readonly string SELECT
              = $"u.{nameof(USERS.LOGIN)}, " +
                $"u.{nameof(USERS.FIRSTNAME)}, " +
                $"u.{nameof(USERS.SURNAME)}, " +
                $"u.{nameof(USERS.IS_ADMIN)} AS IsAdmin, " +
                $"u.{nameof(USERS.EMAIL)}, " +
                $"u.{nameof(USERS.PHONE_NUMBER)} AS PhoneNumber, " +
                $"u.{nameof(USERS.ID_USER)} AS IdUser, " +
                $"m.{nameof(MEETINGS.DATE_MEETING)} AS DateMeeting, " +
                $"m.{nameof(MEETINGS.PLACE)}, " +
                $"m.{nameof(MEETINGS.DESCRIPTION)}, " +
                $"m.{nameof(MEETINGS.QUANTITY)}, " +
                $"m.{nameof(MEETINGS.ID_MEETING)} AS IdMeeting, " +
                $"msg.{nameof(MESSAGES.DATE_ADD)} AS DateAdd, " +
                $"msg.{nameof(MESSAGES.WAITING_TIME)} AS WaitingTime, " +
                $"msg.{nameof(MESSAGES.ANSWER)} ";
        private static readonly string FROM
              = $"{nameof(MESSAGES)} msg " +
                $"JOIN {nameof(MEETINGS)} m ON msg.{nameof(MESSAGES.IDMEETING)} = m.{nameof(MEETINGS.ID_MEETING)} " +
                $"JOIN {nameof(USERS)} u ON msg.{nameof(MESSAGES.IDUSER)} = u.{nameof(USERS.ID_USER)} ";

        public async Task<List<GetMessagesUsersMeetingsResponse>> GetAllMessagesAsync(GetMessagesUsersPaginationRequest getMessagesUsersPaginationRequest, FbTransaction? transaction = null)
        {

            DynamicParameters dynamicParameters = new();
            string WHERE = "1=1";

            if (getMessagesUsersPaginationRequest.IdMeeting is not null)
            {
                WHERE += $"AND msg.{nameof(MESSAGES.IDMEETING)} = @GroupId ";
                dynamicParameters.Add("@GroupId", getMessagesUsersPaginationRequest.IdMeeting);
            }
            if (getMessagesUsersPaginationRequest.IdUser is not null)
            {
                WHERE += $"AND msg.{nameof(MESSAGES.IDUSER)} = @UserId ";
                dynamicParameters.Add("@UserId", getMessagesUsersPaginationRequest.IdUser);
            }
            if (getMessagesUsersPaginationRequest.DateFrom is not null)
            {
                WHERE += $"AND m.{nameof(MEETINGS.DATE_MEETING)} >= @DateFrom ";
                dynamicParameters.Add("@DateFrom", getMessagesUsersPaginationRequest.DateFrom);
            }
            if (getMessagesUsersPaginationRequest.DateTo is not null)
            {
                WHERE += $"AND m.{nameof(MEETINGS.DATE_MEETING)} <= @DateTo ";
                dynamicParameters.Add("@DateTo", getMessagesUsersPaginationRequest.DateTo);
            }
            if (getMessagesUsersPaginationRequest.WaitingTime is not null)
            {
                WHERE += $"AND m.{nameof(MESSAGES.WAITING_TIME)} <= @WaitingTime ";
                dynamicParameters.Add("@WaitingTime", getMessagesUsersPaginationRequest.WaitingTime);
            }

            var query = new QueryBuilder<MESSAGES>()
                .Select(SELECT)
                .From(FROM)
                .Where(WHERE)
                .OrderBy(getMessagesUsersPaginationRequest)
                .Limit(getMessagesUsersPaginationRequest);
            FbConnection db = transaction?.Connection ?? _dbConnection;

            if (transaction == null && db.State != ConnectionState.Open)
            {
                await db.OpenAsync();
            }

            return (await db.QueryAsync<GetMessagesUsersMeetingsResponse>(query.Build(), dynamicParameters, transaction)).AsList();

        }

        public async Task<MESSAGES?> GetMessageByIdAsync(int messageId, FbTransaction? transaction = null)
        {

            var query = new QueryBuilder<MESSAGES>()
                .Select("* ")
                .From("MESSAGES ")
                .Where("ID_MESSAGE = @MessageId ");
            FbConnection db = transaction?.Connection ?? _dbConnection;

            if (transaction == null && db.State != ConnectionState.Open)
            {
                await db.OpenAsync();
            }

            return await db.QuerySingleOrDefaultAsync<MESSAGES>(query.Build(), new { MessageId = messageId }, transaction);

        }

    }
}
