using System.Data;
using Dapper;
using DataLibrary.Entities;
using DataLibrary.Helper;
using DataLibrary.IRepository.Groups;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository.Groups
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

            ReadGroupsRepository readGroupsRepository = new(db);
            try
            {
                var groupTemp = await readGroupsRepository.GetGroupByNameAsync(group.NAME, transaction);
                if (groupTemp != null)
                {
                    throw new Exception("Group with this name already exists");
                }
            }
            catch (NullReferenceException)
            {
                throw;
            }
            await db.ExecuteAsync(insertQuery, group, transaction);
        }
    }
}
