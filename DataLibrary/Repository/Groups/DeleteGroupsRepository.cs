using System.Data;
using Dapper;
using DataLibrary.Entities;
using DataLibrary.IRepository;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository
{
    public class DeleteGroupsRepository(FbConnection dbConnection) : IDeleteGroupsRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;

        public async Task DeleteGroupAsync(int groupId, FbTransaction? transaction = null)
        {

            var deleteBuilder = new QueryBuilder<GROUPS>()
                .Delete("GROUPS ")
                .Where("ID_GROUP = @GroupId ");
            string deleteQuery = deleteBuilder.Build();
            FbConnection db = transaction?.Connection ?? _dbConnection;

            if (transaction == null && db.State != ConnectionState.Open)
            {
                await db.OpenAsync();
            }

            await db.ExecuteAsync(deleteQuery, new { GroupId = groupId }, transaction);

        }
    }
}
