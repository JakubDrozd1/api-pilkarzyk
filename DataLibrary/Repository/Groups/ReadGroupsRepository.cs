using Dapper;
using DataLibrary.Entities;
using DataLibrary.IRepository.Groups;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository
{
    public class ReadGroupsRepository(FbConnection dbConnection) : IReadGroupsRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;

        public async Task<List<Groupe>> GetAllGroupsAsync()
        {
            using FbConnection db = _dbConnection;
            await db.OpenAsync();
            var query = new QueryBuilder<Groupe>()
                .Select("*")
                .From("GROUPS");
            return (await db.QueryAsync<Groupe>(query.Build())).AsList();
        }

        public async Task<Groupe?> GetGroupByIdAsync(int groupId)
        {
            using FbConnection db = _dbConnection;
            await db.OpenAsync();
            var query = new QueryBuilder<Groupe>()
                .Select("*")
                .From("GROUPS")
                .Where("ID_GROUP = @GroupId");
            return await db.QueryFirstOrDefaultAsync<Groupe>(query.Build(), new { GroupId = groupId });
        }
    }
}
