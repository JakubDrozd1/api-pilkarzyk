using System.Data;
using Dapper;
using DataLibrary.Entities;
using DataLibrary.Helper;
using DataLibrary.IRepository.Groups;
using DataLibrary.Model.DTO.Request.Pagination;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository.Groups
{
    public class ReadGroupsRepository(FbConnection dbConnection, FbTransaction? fbTransaction) : IReadGroupsRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;
        private readonly FbTransaction? _fbTransaction = fbTransaction;

        public async Task<List<GROUPS>> GetAllGroupsAsync(GetGroupsPaginationRequest getGroupsPaginationRequest)
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                await _dbConnection.OpenAsync();
            }
            try
            {
                var query = new QueryBuilder<GROUPS>()
                    .Select("* ")
                    .From("GROUPS ")
                    .OrderBy(getGroupsPaginationRequest)
                    .Limit(getGroupsPaginationRequest);
                return (await _dbConnection.QueryAsync<GROUPS>(query.Build())).AsList();
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

        public async Task<GROUPS?> GetGroupByIdAsync(int groupId)
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                await _dbConnection.OpenAsync();
            }
            try
            {
                var query = new QueryBuilder<GROUPS>()
                    .Select("* ")
                    .From("GROUPS ")
                    .Where("ID_GROUP = @GroupId ");
                return await _dbConnection.QuerySingleOrDefaultAsync<GROUPS>(query.Build(), new { GroupId = groupId }, _fbTransaction);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

        public async Task<GROUPS?> GetGroupByNameAsync(string name)
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                await _dbConnection.OpenAsync();
            }
            try
            {
                var query = new QueryBuilder<GROUPS>()
                .Select("* ")
                .From("GROUPS ")
                .Where("NAME = @Name ");
                return await _dbConnection.QuerySingleOrDefaultAsync<GROUPS>(query.Build(), new { Name = name }, _fbTransaction);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
