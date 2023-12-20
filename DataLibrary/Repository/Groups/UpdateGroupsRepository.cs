using System.Data;
using Dapper;
using DataLibrary.Entities;
using DataLibrary.Helper;
using DataLibrary.IRepository.Groups;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository.Groups
{
    public class UpdateGroupsRepository(FbConnection dbConnection) : IUpdateGroupsRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;

        public async Task UpdateGroupAsync(GROUPS group, FbTransaction? transaction = null)
        {

            var updateBuilder = new QueryBuilder<GROUPS>()
                .Update("GROUPS ", group)
                .Where("ID_GROUP = @ID_GROUP ");
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
