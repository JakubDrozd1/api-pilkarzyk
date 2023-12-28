using System.Data;
using Dapper;
using DataLibrary.Entities;
using DataLibrary.Helper;
using DataLibrary.IRepository.Rankings;
using DataLibrary.Model.DTO.Request.Pagination;
using DataLibrary.Model.DTO.Response;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository.Rankings
{
    public class ReadRankingsRepository(FbConnection dbConnection, FbTransaction? fbTransaction) : IReadRankingsRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;
        private readonly FbTransaction? _fbTransaction = fbTransaction;
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

        public async Task<List<GetRankingsUsersGroupsResponse>> GetAllRankingsAsync(GetRankingsUsersGroupsPaginationRequest getRankingsUsersGroupsPaginationRequest)
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                await _dbConnection.OpenAsync();
            }
            try
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

                return (await _dbConnection.QueryAsync<GetRankingsUsersGroupsResponse>(query.Build(), dynamicParameters, _fbTransaction)).AsList();
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

        public async Task<RANKINGS?> GetRankingByIdAsync(int rankingId)
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                await _dbConnection.OpenAsync();
            }
            try
            {
                var query = new QueryBuilder<RANKINGS>()
                    .Select("* ")
                    .From("RANKINGS ")
                    .Where("ID_RANKING = @RankingId ");
                return await _dbConnection.QuerySingleOrDefaultAsync<RANKINGS>(query.Build(), new { RankingId = rankingId }, _fbTransaction);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
