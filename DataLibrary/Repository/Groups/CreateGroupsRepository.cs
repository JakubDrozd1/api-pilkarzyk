using System.Data;
using Dapper;
using DataLibrary.Entities;
using DataLibrary.IRepository;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository
{
    public class CreateGroupsRepository(FbConnection dbConnection) : ICreateGroupsRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;

        public async Task AddGroupAsync(GROUPS group, FbTransaction? transaction = null)
        {

            var insertBuilder = new QueryBuilder<GROUPS>()
                .Insert("GROUPS ", group);
            string insertQuery = insertBuilder.Build();
            FbConnection db = transaction?.Connection ?? _dbConnection;

            if (transaction == null && db.State != ConnectionState.Open)
            {
                await db.OpenAsync();
            }

            await db.ExecuteAsync(insertQuery, group, transaction);

        }
    }
}
