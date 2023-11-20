using Dapper;
using DataLibrary.Entities;
using DataLibrary.IRepository.Groups;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository
{
    public class CreateGroupsRepository(FbConnection dbConnection) : ICreateGroupsRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;

        public async Task AddGroupAsync(Groupe group)
        {
            var insertBuilder = new QueryBuilder<Groupe>()
                .Insert("GROUPS", group);
            string insertQuery = insertBuilder.Build();
            using FbConnection db = _dbConnection;
            await db.OpenAsync();
            await db.ExecuteAsync(insertQuery, group);
        }
    }
}
