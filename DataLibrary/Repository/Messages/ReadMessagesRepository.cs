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
                $"m.{nameof(MEETINGS.DATE_MEETING)} AS DateMeeting, " +
                $"m.{nameof(MEETINGS.PLACE)}, " +
                $"m.{nameof(MEETINGS.DESCRIPTION)}, " +
                $"m.{nameof(MEETINGS.QUANTITY)}, " +
                $"msg.{nameof(MESSAGES.DATE_ADD)} AS DateAdd, " +
                $"msg.{nameof(MESSAGES.ANSWER)} ";
        private static readonly string FROM
              = $"{nameof(MESSAGES)} msg " +
                $"JOIN {nameof(MEETINGS)} m ON msg.{nameof(MESSAGES.IDMEETING)} = m.{nameof(MEETINGS.ID_MEETING)} " +
                $"JOIN {nameof(USERS)} u ON msg.{nameof(MESSAGES.IDUSER)} = u.{nameof(USERS.ID_USER)} ";

        public async Task<List<GetMessagesUsersMeetingsResponse>> GetAllMessagesAsync(GetMessagesUsersPaginationRequest getMeetingsUsersPaginationRequest, FbTransaction? transaction = null)
        {

            DynamicParameters dynamicParameters = new();
            string WHERE = "1=1";

            if (getMeetingsUsersPaginationRequest.IdMeeting is not null)
            {
                WHERE += $"AND msg.{nameof(MESSAGES.IDMEETING)} = @GroupId ";
                dynamicParameters.Add("@GroupId", getMeetingsUsersPaginationRequest.IdMeeting);
            }

            if (getMeetingsUsersPaginationRequest.IdUser is not null)
            {
                WHERE += $"AND msg.{nameof(MESSAGES.IDUSER)} = @UserId ";
                dynamicParameters.Add("@UserId", getMeetingsUsersPaginationRequest.IdUser);
            }

            var query = new QueryBuilder<MESSAGES>()
                .Select(SELECT)
                .From(FROM)
                .Where(WHERE)
                .OrderBy(getMeetingsUsersPaginationRequest)
                .Limit(getMeetingsUsersPaginationRequest);
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
