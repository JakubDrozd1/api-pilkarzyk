using System.Data;
using Dapper;
using DataLibrary.Entities;
using DataLibrary.Helper;
using DataLibrary.IRepository.Meetings;
using DataLibrary.Model.DTO.Request.Pagination;
using DataLibrary.Model.DTO.Request.TableRequest;
using DataLibrary.Model.DTO.Response;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository.Meetings
{
    public class ReadMeetingsRepository(FbConnection dbConnection, FbTransaction? fbTransaction) : IReadMeetingsRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;
        private readonly FbTransaction? _fbTransaction = fbTransaction;
        private static readonly string SELECT
              = $"g.{nameof(GROUPS.NAME)}, " +
                $"g.{nameof(GROUPS.ID_GROUP)} AS IdGroup, " +
                $"m.{nameof(MEETINGS.DATE_MEETING)} AS DateMeeting, " +
                $"m.{nameof(MEETINGS.ID_MEETING)} AS IdMeeting, " +
                $"m.{nameof(MEETINGS.PLACE)}, " +
                $"m.{nameof(MEETINGS.DESCRIPTION)}, " +
                $"m.{nameof(MEETINGS.IDAUTHOR)}, " +
                $"m.{nameof(MEETINGS.IS_INDEPENDENT)} AS IsIndependent, " +
                $"m.{nameof(MEETINGS.QUANTITY)} ";
        private string FROM
              = $"{nameof(MEETINGS)} m " +
                $"JOIN {nameof(GROUPS)} g ON m.{nameof(MEETINGS.IDGROUP)} = g.{nameof(GROUPS.ID_GROUP)} ";

        public async Task<List<GetMeetingGroupsResponse>> GetAllMeetingsAsync(GetMeetingsGroupsPaginationRequest getMeetingsRequest)
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                await _dbConnection.OpenAsync();
            }
            try
            {
                DynamicParameters dynamicParameters = new();
                string WHERE = "1=1 ";
                if (getMeetingsRequest.IdAuthor is not null)
                {
                    WHERE += $"AND m.{nameof(MEETINGS.IDAUTHOR)} = @AuthorId ";
                    dynamicParameters.Add("@AuthorId", getMeetingsRequest.IdAuthor);
                }
                if (getMeetingsRequest.IdGroup is not null)
                {
                    WHERE += $"AND m.{nameof(MEETINGS.IDGROUP)} = @GroupId ";
                    dynamicParameters.Add("@GroupId", getMeetingsRequest.IdGroup);
                }
                if (getMeetingsRequest.IdUser is not null)
                {
                    WHERE += $"AND msg.{nameof(MESSAGES.IDUSER)} = @UserId ";
                    dynamicParameters.Add("@UserId", getMeetingsRequest.IdUser);
                }
                if (getMeetingsRequest.Answer is not null)
                {
                    WHERE += $"AND msg.{nameof(MESSAGES.ANSWER)} = @Answer ";
                    dynamicParameters.Add("@Answer", getMeetingsRequest.Answer);
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
                if (getMeetingsRequest.WithMessages)
                {
                    FROM += $"JOIN {nameof(MESSAGES)} msg ON m.{nameof(MEETINGS.ID_MEETING)} = msg.{nameof(MESSAGES.IDMEETING)} ";
                }
                var query = new QueryBuilder<GetMeetingGroupsResponse>()
                    .Select(SELECT)
                    .From(FROM)
                    .Where(WHERE)
                    .OrderBy(getMeetingsRequest)
                    .Limit(getMeetingsRequest);
                return (await _dbConnection.QueryAsync<GetMeetingGroupsResponse>(query.Build(), dynamicParameters, _fbTransaction)).AsList();
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

        public async Task<GetMeetingGroupsResponse?> GetMeetingByIdAsync(int meetingId)
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                await _dbConnection.OpenAsync();
            }
            try
            {
                var query = new QueryBuilder<GetMeetingGroupsResponse>()
                    .Select(SELECT)
                    .From($"{nameof(MEETINGS)} m " +
                    $"JOIN {nameof(GROUPS)} g ON m.{nameof(MEETINGS.IDGROUP)} = g.{nameof(GROUPS.ID_GROUP)} ")
                    .Where("m.ID_MEETING = @MeetingId ");
                return await _dbConnection.QuerySingleOrDefaultAsync<GetMeetingGroupsResponse>(query.Build(), new { MeetingId = meetingId }, _fbTransaction);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

        public async Task<MEETINGS?> GetMeeting(GetMeetingRequest getMeetingRequest)
        {
            try
            {
                DynamicParameters dynamicParameters = new();
                string WHERE = "1=1 ";

                if (getMeetingRequest.IDGROUP is not null)
                {
                    WHERE += $"AND {nameof(MEETINGS.IDGROUP)} = @GroupId ";
                    dynamicParameters.Add("@GroupId", getMeetingRequest.IDGROUP);
                }
                if (getMeetingRequest.PLACE is not null)
                {
                    WHERE += $"AND {nameof(MEETINGS.PLACE)} = @Place ";
                    dynamicParameters.Add("@Place", getMeetingRequest.PLACE);
                }
                if (getMeetingRequest.DATE_MEETING is not null)
                {
                    WHERE += $"AND {nameof(MEETINGS.DATE_MEETING)} = @DateMeeting ";
                    dynamicParameters.Add("@DateMeeting", getMeetingRequest.DATE_MEETING);
                }
                if (getMeetingRequest.QUANTITY is not null)
                {
                    WHERE += $"AND {nameof(MEETINGS.QUANTITY)} = @Quantity ";
                    dynamicParameters.Add("@Quantity", getMeetingRequest.QUANTITY);
                }
                if (getMeetingRequest.DESCRIPTION is not null)
                {
                    WHERE += $"AND {nameof(MEETINGS.DESCRIPTION)} = @Description ";
                    dynamicParameters.Add("@Description", getMeetingRequest.DESCRIPTION);
                }
                var query = new QueryBuilder<MEETINGS>()
                    .Select("* ")
                    .From("MEETINGS ")
                    .Where(WHERE);
                return await _dbConnection.QuerySingleOrDefaultAsync<MEETINGS>(query.Build(), dynamicParameters, _fbTransaction);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
