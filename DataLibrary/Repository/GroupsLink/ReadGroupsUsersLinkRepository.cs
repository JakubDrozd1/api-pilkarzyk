using Dapper;
using DataLibrary.Entities;
using DataLibrary.Helper;
using DataLibrary.IRepository.GroupsLink;
using FirebirdSql.Data.FirebirdClient;
using System.Data;

namespace DataLibrary.Repository.GroupsLink
{
    public class ReadGroupsUsersLinkRepository(FbConnection dbConnection, FbTransaction? fbTransaction) : IReadGroupsUsersLinkRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;
        private readonly FbTransaction? _fbTransaction = fbTransaction;

        public async Task<GROUPS_USERS_LINK?> ReadGroupLinkAsync(int groupId = 1, int userId = 2)
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                await _dbConnection.OpenAsync();
            }
            try
            {
                var query = new QueryBuilder<GROUPS_USERS_LINK>()
                        .Select("* ")
                        .From("GROUPS_USERS_LINK")
                        .Where($"{nameof(GROUPS_USERS_LINK.IDGROUP)} = {groupId} AND {nameof(GROUPS_USERS_LINK.IDUSER)} = {userId}");
                return (await _dbConnection.QueryAsync<GROUPS_USERS_LINK>(query.Build(), transaction: _fbTransaction)).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

        public async Task<GROUPS_USERS_LINK?> ReadLinkByCodeAsync(string code)
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                await _dbConnection.OpenAsync();
            }
            try
            {
                var query = new QueryBuilder<GROUPS_USERS_LINK>()
                        .Select("* ")
                        .From("GROUPS_USERS_LINK")
                        .Where($"{nameof(GROUPS_USERS_LINK.CODE)} = '{code}'");
                return (await _dbConnection.QueryAsync<GROUPS_USERS_LINK>(query.Build(), transaction: _fbTransaction)).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
