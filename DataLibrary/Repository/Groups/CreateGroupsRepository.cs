using System.Data;
using Dapper;
using DataLibrary.Entities;
using DataLibrary.Helper;
using DataLibrary.IRepository.Groups;
using DataLibrary.Model.DTO.Request;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository.Groups
{
    public class CreateGroupsRepository(FbConnection dbConnection, FbTransaction? fbTransaction) : ICreateGroupsRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;
        private readonly FbTransaction? _fbTransaction = fbTransaction;

        public async Task AddGroupAsync(GetGroupRequest group)
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                await _dbConnection.OpenAsync();
            }
            try
            {
                var insertBuilder = new QueryBuilder<GROUPS>()
                    .Insert("GROUPS ", group);
                string insertQuery = insertBuilder.Build();
                await _dbConnection.ExecuteAsync(insertQuery, group, _fbTransaction);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}"); ;
            }
        }
    }
}
