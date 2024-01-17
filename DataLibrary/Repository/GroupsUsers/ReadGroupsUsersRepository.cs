using System.Data;
using Dapper;
using DataLibrary.Entities;
using DataLibrary.Helper;
using DataLibrary.IRepository.GroupsUsers;
using DataLibrary.Model.DTO.Request.Pagination;
using DataLibrary.Model.DTO.Response;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository.GroupsUsers
{
    public class ReadGroupsUsersRepository(FbConnection dbConnection, FbTransaction? fbTransaction) : IReadGroupsUsersRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;
        private readonly FbTransaction? _fbTransaction = fbTransaction;
        private static readonly string SELECT
              = $"g.{nameof(GROUPS.NAME)}, " +
                $"g.{nameof(GROUPS.ID_GROUP)} AS IdGroup, " +
                $"gu.{nameof(GROUPS_USERS.ACCOUNT_TYPE)} AS AccountType, " +
                $"gu.{nameof(GROUPS_USERS.ID_GROUP_USER)} AS IdGroupUser, " +
                $"u.{nameof(USERS.LOGIN)}, " +
                $"u.{nameof(USERS.ID_USER)} AS IdUser, " +
                $"u.{nameof(USERS.EMAIL)}, " +
                $"u.{nameof(USERS.FIRSTNAME)}, " +
                $"u.{nameof(USERS.SURNAME)}, " +
                $"u.{nameof(USERS.PHONE_NUMBER)} AS PhoneNumber, " +
                $"u.{nameof(USERS.AVATAR)}, " +
                $"u.{nameof(USERS.GROUP_COUNTER)} AS GroupCounter, " +
                $"u.{nameof(USERS.IS_ADMIN)} AS IsAdmin ";
        private static readonly string FROM
              = $"{nameof(GROUPS_USERS)} gu " +
                $"JOIN {nameof(GROUPS)} g ON gu.{nameof(GROUPS_USERS.IDGROUP)} = g.{nameof(GROUPS.ID_GROUP)} " +
                $"JOIN {nameof(USERS)} u ON gu.{nameof(GROUPS_USERS.IDUSER)} = u.{nameof(USERS.ID_USER)} ";

        public async Task<List<GetGroupsUsersResponse>> GetListGroupsUserAsync(GetUsersGroupsPaginationRequest getUsersGroupsRequest)
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                await _dbConnection.OpenAsync();
            }
            try
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
                return (await _dbConnection.QueryAsync<GetGroupsUsersResponse>(query.Build(), dynamicParameters, _fbTransaction)).AsList();
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

        public async Task<GetGroupsUsersResponse?> GetUserWithGroup(int groupId, int userId)
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                await _dbConnection.OpenAsync();
            }
            try
            {
                var query = new QueryBuilder<GetGroupsUsersResponse>()
                    .Select(SELECT)
                    .From(FROM)
                    .Where("gu.IDGROUP = @GroupId AND gu.IDUSER = @UserId ");
                return await _dbConnection.QuerySingleOrDefaultAsync<GetGroupsUsersResponse?>(query.Build(), new { UserId = userId, GroupId = groupId }, _fbTransaction);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
