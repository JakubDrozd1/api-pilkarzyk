using Dapper;
using DataLibrary.Entities;
using DataLibrary.IRepository;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository
{
    public class DeleteGroupsRepository(FbConnection dbConnection) : IDeleteGroupsRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;

        public async Task DeleteGroupAsync(int groupId)
        {
            var deleteBuilder = new QueryBuilder<Groupe>()
                .Delete("GROUPS")
                .Where("ID_GROUP = @GroupId");
            string deleteQuery = deleteBuilder.Build();
            using FbConnection db = _dbConnection;
            await db.OpenAsync();
            await db.ExecuteAsync(deleteQuery, new { GroupId = groupId });
        }
    }
}
