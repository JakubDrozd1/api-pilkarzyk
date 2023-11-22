using System.Data;
using BLLLibrary.Model.DTO.Response;
using Dapper;
using DataLibrary.Entities;
using DataLibrary.IRepository;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository
{
    public class ReadGroupsUsersRepository(FbConnection dbConnection) : IReadGroupsUsersRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;
        private static readonly string SELECT = "g.NAME, u.LOGIN, u.PASSWORD, u.FIRSTNAME, u.SURNAME, u.ACCOUNT_TYPE, u.EMAIL, u.PHONE_NUMBER";
        private static readonly string FROM = "GROUPS_USERS gu JOIN GROUPS g ON gu.IDGROUP = g.ID_GROUP JOIN USERS u ON gu.IDUSER = u.ID_USER";

        public async Task<List<GetGroupsWithUsersResponse>> GetAllGroupsFromUserAsync(int userId, FbTransaction? transaction = null)
        {
            var query = new QueryBuilder<GroupsUsers>()
                .Select(SELECT)
                .From(FROM)
                .Where("gu.IDUSER = @UserId");
            FbConnection db = transaction?.Connection ?? _dbConnection;
            if (db.State != ConnectionState.Open && transaction == null)
            {
                await db.OpenAsync();
            }
            return (await db.QueryAsync<GetGroupsWithUsersResponse>(query.Build(), new { UserId = userId })).AsList();
        }

        public async Task<List<GetGroupsWithUsersResponse>> GetAllUsersFromGroupAsync(int groupId, FbTransaction? transaction = null)
        {
            var query = new QueryBuilder<GroupsUsers>()
                .Select(SELECT)
                .From(FROM)
                .Where("gu.IDGROUP = @GroupId");
            FbConnection db = transaction?.Connection ?? _dbConnection;
            if (db.State != ConnectionState.Open && transaction == null)
            {
                await db.OpenAsync();
            }
            return (await db.QueryAsync<GetGroupsWithUsersResponse>(query.Build(), new { GroupId = groupId })).AsList();
        }

        public async Task<GetGroupsWithUsersResponse?> GetUserWithGroup(int groupId, int userId, FbTransaction? transaction = null)
        {
            var query = new QueryBuilder<GroupsUsers>()
                .Select(SELECT)
                .From(FROM)
                .Where("gu.IDGROUP = @GroupId AND gu.IDUSER = @UserId");
            FbConnection db = transaction?.Connection ?? _dbConnection;
            if (transaction == null && db.State != ConnectionState.Open)
            {
                await db.OpenAsync();
            }
            return await db.QuerySingleOrDefaultAsync<GetGroupsWithUsersResponse?>(query.Build(), new { UserId = userId, GroupId = groupId }, transaction);
        }
    }
}
