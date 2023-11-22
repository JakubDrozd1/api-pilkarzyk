using System.Data;
using Dapper;
using DataLibrary.Entities;
using DataLibrary.IRepository;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository
{
    public class ReadGroupsRepository(FbConnection dbConnection) : IReadGroupsRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;

        public async Task<List<Groupe>> GetAllGroupsAsync()
        {
            using FbConnection db = _dbConnection;
            if (db.State != ConnectionState.Open)
                await db.OpenAsync();
            var query = new QueryBuilder<Groupe>()
                .Select("*")
                .From("GROUPS");
            return (await db.QueryAsync<Groupe>(query.Build())).AsList();
        }

        public async Task<Groupe?> GetGroupByIdAsync(int groupId, FbTransaction? transaction = null)
        {
            var query = new QueryBuilder<Groupe>()
                .Select("*")
                .From("GROUPS")
                .Where("ID_GROUP = @GroupId");
            FbConnection db = transaction?.Connection ?? _dbConnection;
            if (transaction == null && db.State != ConnectionState.Open)
            {
                await db.OpenAsync();
            }            
            return await db.QuerySingleOrDefaultAsync<Groupe>(query.Build(), new { GroupId = groupId }, transaction);
        }
    }
}
