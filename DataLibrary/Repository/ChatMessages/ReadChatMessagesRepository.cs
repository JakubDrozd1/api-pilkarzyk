using Dapper;
using System.Data;
using DataLibrary.Entities;
using DataLibrary.Helper;
using DataLibrary.IRepository.ChatMessages;
using DataLibrary.Model.DTO.Request.Pagination;
using DataLibrary.Model.DTO.Response;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository.ChatMessages
{
    public class ReadChatMessagesRepository(FbConnection dbConnection, FbTransaction? fbTransaction) : IReadChatMessagesRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;
        private readonly FbTransaction? _fbTransaction = fbTransaction;
        private static readonly string SELECT
            = $"cm.{nameof(CHAT_MESSAGES.DATE_SEND)} AS DateSend, " +
            $"cm.{nameof(CHAT_MESSAGES.CHAT_MESSAGE)} AS ChatMessage, " +
            $"u.{nameof(USERS.ID_USER)} AS IdUser, " +
            $"u.{nameof(USERS.LOGIN)}, " +
            $"u.{nameof(USERS.FIRSTNAME)}, " +
            $"u.{nameof(USERS.AVATAR)}, " +
            $"u.{nameof(USERS.SURNAME)} ";
        private static readonly string FROM
              = $"{nameof(CHAT_MESSAGES)} cm " +
                $"JOIN {nameof(MEETINGS)} m ON cm.{nameof(CHAT_MESSAGES.IDMEETING)} = m.{nameof(MEETINGS.ID_MEETING)} " +
                $"JOIN {nameof(USERS)} u ON cm.{nameof(CHAT_MESSAGES.IDUSER)} = u.{nameof(USERS.ID_USER)} ";
        public async Task<List<GetChatMessagesResponse>> GetAllChatMessagesFromMeeting(GetChatMessagesPaginationRequest getGroupsPaginationRequest)
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                await _dbConnection.OpenAsync();
            }
            try
            {
                DynamicParameters dynamicParameters = new();
                string WHERE = "1=1 ";

                if (getGroupsPaginationRequest.IdMeeting is not null)
                {
                    WHERE += $"AND cm.{nameof(CHAT_MESSAGES.IDMEETING)} = @MeetingId ";
                    dynamicParameters.Add("@MeetingId", getGroupsPaginationRequest.IdMeeting);
                }
                if (getGroupsPaginationRequest.DateFrom is not null)
                {
                    WHERE += $"AND m.{nameof(CHAT_MESSAGES.DATE_SEND)} >= @DateFrom ";
                    dynamicParameters.Add("@DateFrom", getGroupsPaginationRequest.DateFrom);
                }
                if (getGroupsPaginationRequest.DateTo is not null)
                {
                    WHERE += $"AND m.{nameof(CHAT_MESSAGES.DATE_SEND)} <= @DateTo ";
                    dynamicParameters.Add("@DateTo", getGroupsPaginationRequest.DateTo);
                }
                var query = new QueryBuilder<GetGroupInviteResponse>()
                    .Select(SELECT)
                    .From(FROM)
                    .Where(WHERE)
                    .OrderBy(getGroupsPaginationRequest)
                    .Limit(getGroupsPaginationRequest);

                return (await _dbConnection.QueryAsync<GetChatMessagesResponse>(query.Build(), dynamicParameters, _fbTransaction)).AsList();
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
