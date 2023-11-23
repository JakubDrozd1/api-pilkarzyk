using System.Data;
using Dapper;
using DataLibrary.Entities;
using DataLibrary.IRepository;
using DataLibrary.Model.DTO.Request;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository
{
    public class ReadGroupsRepository(FbConnection dbConnection) : IReadGroupsRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;

        public async Task<List<GROUPS>> GetAllGroupsAsync(GetGroupsPaginationRequest getGroupsPaginationRequest, FbTransaction? transaction = null)
        {

            var query = new QueryBuilder<GROUPS>()
                .Select("* ")
                .From("GROUPS ")
                .OrderBy(getGroupsPaginationRequest)
                .Limit(getGroupsPaginationRequest);
            FbConnection db = transaction?.Connection ?? _dbConnection;

            if (transaction == null && db.State != ConnectionState.Open)
            {
                await db.OpenAsync();
            }

            return (await db.QueryAsync<GROUPS>(query.Build(), transaction)).AsList();

        }

        public async Task<GROUPS?> GetGroupByIdAsync(int groupId, FbTransaction? transaction = null)
        {

            var query = new QueryBuilder<GROUPS>()
                .Select("* ")
                .From("GROUPS ")
                .Where("ID_GROUP = @GroupId ");
            FbConnection db = transaction?.Connection ?? _dbConnection;

            if (transaction == null && db.State != ConnectionState.Open)
            {
                await db.OpenAsync();
            }

            return await db.QuerySingleOrDefaultAsync<GROUPS>(query.Build(), new { GroupId = groupId }, transaction);

        }
    }
}
