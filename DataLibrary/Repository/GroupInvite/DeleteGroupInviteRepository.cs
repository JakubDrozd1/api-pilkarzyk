using Dapper;
using DataLibrary.Entities;
using DataLibrary.Helper;
using System.Data;
using DataLibrary.IRepository.GroupInvite;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository.GroupInvite
{
    public class DeleteGroupInviteRepository(FbConnection dbConnection, FbTransaction? fbTransaction) : IDeleteGroupInviteRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;
        private readonly FbTransaction? _fbTransaction = fbTransaction;

        public async Task DeleteGroupInviteAsync(int groupInviteId)
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                await _dbConnection.OpenAsync();
            }
            try
            {
                var deleteBuilder = new QueryBuilder<GROUP_INVITE>()
                    .Delete("GROUP_INVITE ")
                    .Where("ID_GROUP_INVITE = @GroupInviteId ");
                string deleteQuery = deleteBuilder.Build();
                await _dbConnection.ExecuteAsync(deleteQuery, new { GroupInviteId = groupInviteId }, _fbTransaction);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }

        }
    }
}
