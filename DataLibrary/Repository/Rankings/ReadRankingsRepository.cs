using System.Data;
using Dapper;
using DataLibrary.Entities;
using DataLibrary.Helper;
using DataLibrary.IRepository.Rankings;
using DataLibrary.Model.DTO.Request;
using DataLibrary.Model.DTO.Response;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository.Rankings
{
    public class ReadRankingsRepository(FbConnection dbConnection) : IReadRankingsRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;
        private static readonly string SELECT
              = $"g.{nameof(GROUPS.NAME)}, " +
                $"u.{nameof(USERS.LOGIN)}, " +
                $"u.{nameof(USERS.FIRSTNAME)}, " +
                $"u.{nameof(USERS.SURNAME)}, " +
                $"u.{nameof(USERS.IS_ADMIN)} AS IsAdmin, " +
                $"u.{nameof(USERS.EMAIL)}, " +
                $"u.{nameof(USERS.PHONE_NUMBER)} AS PhoneNumber, " +
                $"r.{nameof(RANKINGS.DATE_MEETING)} AS DateMeeting, " +
                $"r.{nameof(RANKINGS.POINT)} ";
        private static readonly string FROM
              = $"{nameof(RANKINGS)} r " +
                $"LEFT JOIN {nameof(GROUPS)} g ON r.{nameof(RANKINGS.IDGROUP)} = g.{nameof(GROUPS.ID_GROUP)} " +
                $"LEFT JOIN {nameof(USERS)} u ON r.{nameof(RANKINGS.IDUSER)} = u.{nameof(USERS.ID_USER)} ";

        public async Task<List<GetRankingsUsersGroupsResponse>> GetAllRankingsAsync(GetRankingsUsersGroupsPaginationRequest getRankingsUsersGroupsPaginationRequest, FbTransaction? transaction = null)
        {

            DynamicParameters dynamicParameters = new();
            string WHERE = "1=1";

            if (getRankingsUsersGroupsPaginationRequest.IdGroup is not null)
            {
                WHERE += $"AND r.{nameof(RANKINGS.IDGROUP)} = @GroupId ";
                dynamicParameters.Add("@GroupId", getRankingsUsersGroupsPaginationRequest.IdGroup);
            }

            if (getRankingsUsersGroupsPaginationRequest.IdUser is not null)
            {
                WHERE += $"AND r.{nameof(RANKINGS.IDUSER)} = @UserId ";
                dynamicParameters.Add("@UserId", getRankingsUsersGroupsPaginationRequest.IdUser);
            }

            if (getRankingsUsersGroupsPaginationRequest.DateFrom is not null)
            {
                WHERE += $"AND r.{nameof(RANKINGS.DATE_MEETING)} >= @DateFrom ";
                dynamicParameters.Add("@DateFrom", getRankingsUsersGroupsPaginationRequest.DateFrom);
            }

            if (getRankingsUsersGroupsPaginationRequest.DateTo is not null)
            {
                WHERE += $"AND r.{nameof(RANKINGS.DATE_MEETING)} <= @DateTo ";
                dynamicParameters.Add("@DateTo", getRankingsUsersGroupsPaginationRequest.DateTo);
            }

            var query = new QueryBuilder<GetRankingsUsersGroupsResponse>()
                .Select(SELECT)
                .From(FROM)
                .Where(WHERE)
                .OrderBy(getRankingsUsersGroupsPaginationRequest)
                .Limit(getRankingsUsersGroupsPaginationRequest);
            FbConnection db = transaction?.Connection ?? _dbConnection;

            if (transaction == null && db.State != ConnectionState.Open)
            {
                await db.OpenAsync();
            }

            return (await db.QueryAsync<GetRankingsUsersGroupsResponse>(query.Build(), dynamicParameters, transaction)).AsList();

        }

        public async Task<RANKINGS?> GetRankingByIdAsync(int rankingId, FbTransaction? transaction = null)
        {

            var query = new QueryBuilder<RANKINGS>()
                .Select("* ")
                .From("RANKINGS ")
                .Where("ID_RANKING = @RankingId ");
            FbConnection db = transaction?.Connection ?? _dbConnection;

            if (transaction == null && db.State != ConnectionState.Open)
            {
                await db.OpenAsync();
            }

            return await db.QuerySingleOrDefaultAsync<RANKINGS>(query.Build(), new { RankingId = rankingId }, transaction);

        }
    }
}
