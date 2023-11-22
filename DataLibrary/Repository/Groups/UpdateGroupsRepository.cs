using System.Data;
using Dapper;
using DataLibrary.Entities;
using DataLibrary.IRepository;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository
{
    public class UpdateGroupsRepository(FbConnection dbConnection) : IUpdateGroupsRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;

        public async Task UpdateGroupAsync(Groupe group, FbTransaction? transaction = null)
        {
            var updateBuilder = new QueryBuilder<Groupe>()
                .Update("GROUPS", group)
                .Where("ID_GROUP = @ID_GROUP");
            string updateQuery = updateBuilder.Build();
            FbConnection db = transaction?.Connection ?? _dbConnection;
            if (transaction == null && db.State != ConnectionState.Open)
            {
                await db.OpenAsync();
            }
            await db.ExecuteAsync(updateQuery, group, transaction);
        }
    }
}
