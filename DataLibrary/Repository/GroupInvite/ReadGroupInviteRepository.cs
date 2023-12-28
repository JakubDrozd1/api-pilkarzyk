using Dapper;
using DataLibrary.Entities;
using DataLibrary.Helper;
using System.Data;
using DataLibrary.IRepository.GroupInvite;
using DataLibrary.Model.DTO.Response;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository.GroupInvite
{
    public class ReadGroupInviteRepository(FbConnection dbConnection, FbTransaction? fbTransaction) : IReadGroupInviteRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;
        private readonly FbTransaction? _fbTransaction = fbTransaction;
        private static readonly string SELECT
              = $"g.{nameof(GROUPS.NAME)}, " +
                $"gi.{nameof(GROUP_INVITE.IDUSER)}, " +
                $"gi.{nameof(GROUP_INVITE.IDAUTHOR)}, " +
                $"gi.{nameof(GROUP_INVITE.IDGROUP)}, " +
                $"gi.{nameof(GROUP_INVITE.ID_GROUP_INVITE)} AS IdGroupInvite, " +
                $"gi.{nameof(GROUP_INVITE.DATE_ADD)} AS DateAdd ";
        private static readonly string FROM
              = $"{nameof(GROUP_INVITE)} gi " +
                $"JOIN {nameof(GROUPS)} g ON gi.{nameof(GROUP_INVITE.IDGROUP)} = g.{nameof(GROUPS.ID_GROUP)} ";

        public async Task<List<GetGroupInviteResponse?>> GetGroupInviteByIdUserAsync(int userId)
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                await _dbConnection.OpenAsync();
            }
            try
            {
                var query = new QueryBuilder<GetGroupInviteResponse>()
                    .Select(SELECT)
                    .From(FROM)
                    .Where("IDUSER = @UserId ");
                return (await _dbConnection.QueryAsync<GetGroupInviteResponse?>(query.Build(), new { UserId = userId }, _fbTransaction)).AsList();
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
