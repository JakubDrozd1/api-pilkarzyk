using System.Data;
using Dapper;
using DataLibrary.Entities;
using DataLibrary.IRepository.GroupsUsers;
using DataLibrary.Model.DTO.Request;
using DataLibrary.Model.DTO.Response;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository.GroupsUsers
{
    public class ReadGroupsUsersRepository(FbConnection dbConnection) : IReadGroupsUsersRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;
        private static readonly string SELECT
              = $"g.{nameof(GROUPS.NAME)}, " +
                $"g.{nameof(GROUPS.ID_GROUP)} AS IdGroup, " +
                $"gu.{nameof(GROUPS_USERS.ACCOUNT_TYPE)} AS AccountType, " +
                $"u.{nameof(USERS.LOGIN)}, " +
                $"u.{nameof(USERS.ID_USER)} AS IdUser, " +
                $"u.{nameof(USERS.EMAIL)}, " +
                $"u.{nameof(USERS.FIRSTNAME)}, " +
                $"u.{nameof(USERS.SURNAME)}, " +
                $"u.{nameof(USERS.PHONE_NUMBER)} AS PhoneNumber, " +
                $"u.{nameof(USERS.AVATAR)}, " +
                $"u.{nameof(USERS.IS_ADMIN)} AS IsAdmin ";
        private static readonly string FROM
              = $"{nameof(GROUPS_USERS)} gu " +
                $"JOIN {nameof(GROUPS)} g ON gu.{nameof(GROUPS_USERS.IDGROUP)} = g.{nameof(GROUPS.ID_GROUP)} " +
                $"JOIN {nameof(USERS)} u ON gu.{nameof(GROUPS_USERS.IDUSER)} = u.{nameof(USERS.ID_USER)} ";

        public async Task<List<GetGroupsUsersResponse>> GetListGroupsUserAsync(GetUsersGroupsPaginationRequest getUsersGroupsRequest, FbTransaction? transaction = null)
        {
            DynamicParameters dynamicParameters = new();
            string WHERE = "1=1 ";

            if (getUsersGroupsRequest.IdGroup is not null)
            {
                WHERE += $"AND gu.{nameof(GROUPS_USERS.IDGROUP)} = @GroupId ";
                dynamicParameters.Add("@GroupId", getUsersGroupsRequest.IdGroup);
            }

            if (getUsersGroupsRequest.IdUser is not null)
            {
                WHERE += $"AND gu.{nameof(GROUPS_USERS.IDUSER)} = @UserId ";
                dynamicParameters.Add("@UserId", getUsersGroupsRequest.IdUser);
            }

            var query = new QueryBuilder<GetGroupsUsersResponse>()
                .Select(SELECT)
                .From(FROM)
                .Where(WHERE)
                .OrderBy(getUsersGroupsRequest)
                .Limit(getUsersGroupsRequest);

            FbConnection db = transaction?.Connection ?? _dbConnection;
            if (db.State != ConnectionState.Open && transaction == null)
            {
                await db.OpenAsync();
            }
            FbTransaction localTransaction = transaction ?? await db.BeginTransactionAsync();

            return (await db.QueryAsync<GetGroupsUsersResponse>(query.Build(), dynamicParameters, localTransaction)).AsList();
        }

        public async Task<GetGroupsUsersResponse?> GetUserWithGroup(int groupId, int userId, FbTransaction? transaction = null)
        {

            var query = new QueryBuilder<GetGroupsUsersResponse>()
                .Select(SELECT)
                .From(FROM)
                .Where("gu.IDGROUP = @GroupId AND gu.IDUSER = @UserId ");
            FbConnection db = transaction?.Connection ?? _dbConnection;

            if (transaction == null && db.State != ConnectionState.Open)
            {
                await db.OpenAsync();
            }

            return await db.QuerySingleOrDefaultAsync<GetGroupsUsersResponse?>(query.Build(), new { UserId = userId, GroupId = groupId }, transaction);

        }
    }
}
