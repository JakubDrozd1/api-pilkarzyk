using Dapper;
using System.Data;
using DataLibrary.Helper;
using DataLibrary.IRepository.GroupInvite;
using FirebirdSql.Data.FirebirdClient;
using DataLibrary.Model.DTO.Request.TableRequest;

namespace DataLibrary.Repository.GroupInvite
{
    public class CreateGroupInviteRepository(FbConnection dbConnection, FbTransaction? fbTransaction) : ICreateGroupInviteRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;
        private readonly FbTransaction? _fbTransaction = fbTransaction;

        public async Task AddGroupInviteAsync(GetGroupInviteRequest getGroupInviteRequest)
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                await _dbConnection.OpenAsync();
            }
            try
            {
                var insertBuilder = new QueryBuilder<GetGroupInviteRequest>()
                    .Insert("GROUP_INVITE ", getGroupInviteRequest);
                string insertQuery = insertBuilder.Build();
                await _dbConnection.ExecuteAsync(insertQuery, getGroupInviteRequest, _fbTransaction);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
