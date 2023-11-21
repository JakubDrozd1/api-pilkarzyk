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
        private static readonly string SELECT = "g.NAME, u.LOGIN, u.PASSWORD, u.FIRSTNAME, u.SURNAME, u.ACCOUNT_TYPE, u.EMAIL, U.PHONE_NUMBER";
        private static readonly string FROM = "GROUPS_USERS gu JOIN GROUPS g ON gu.IDGROUP = g.ID_GROUP JOIN USERS u ON gu.IDUSER = u.ID_USER";

        public async Task<List<GetGroupsWithUsersResponse>> GetAllGroupsFromUserAsync(int userId)
        {
            using FbConnection db = _dbConnection;
            await db.OpenAsync();
            var query = new QueryBuilder<GroupsUsers>()
                .Select(SELECT)
                .From(FROM)
                .Where("gu.IDUSER = @UserId");
            return (await db.QueryAsync<GetGroupsWithUsersResponse>(query.Build(), new { UserId = userId })).AsList();
        }

        public async Task<List<GetGroupsWithUsersResponse>> GetAllUsersFromGroupAsync(int groupId)
        {
            using FbConnection db = _dbConnection;
            await db.OpenAsync();
            var query = new QueryBuilder<GroupsUsers>()
                .Select(SELECT)
                .From(FROM)
                .Where("gu.IDGROUP = @GroupId");
            return (await db.QueryAsync<GetGroupsWithUsersResponse>(query.Build(), new { GroupId = groupId })).AsList();
        }

        public async Task<GetGroupsWithUsersResponse?> GetUserWithGroup(int groupId, int userId)
        {
            using FbConnection db = _dbConnection;
            await db.OpenAsync();
            var query = new QueryBuilder<GroupsUsers>()
                .Select(SELECT)
                .From(FROM)
                .Where("gu.IDGROUP = @GroupId AND gu.IDUSER = @UserId");
            return await db.QueryFirstOrDefaultAsync<GetGroupsWithUsersResponse?>(query.Build(), new { UserId = userId, GroupId = groupId });
        }
    }
}
