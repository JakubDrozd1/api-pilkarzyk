using Dapper;
using DataLibrary.Entities;
using DataLibrary.Helper;
using System.Data;
using DataLibrary.IRepository.GroupInvite;
using DataLibrary.Model.DTO.Response;
using FirebirdSql.Data.FirebirdClient;
using DataLibrary.Model.DTO.Request.Pagination;

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
                $"gi.{nameof(GROUP_INVITE.DATE_ADD)} AS DateAdd, " +
                $"u1.{nameof(USERS.FIRSTNAME)} AS FirstnameAuthor, " +
                $"u1.{nameof(USERS.SURNAME)} AS SurnameAuthor, " +
                $"u2.{nameof(USERS.FIRSTNAME)}, " +
                $"u2.{nameof(USERS.SURNAME)} ";
        private static readonly string FROM
              = $"{nameof(GROUP_INVITE)} gi " +
                $"JOIN {nameof(GROUPS)} g ON gi.{nameof(GROUP_INVITE.IDGROUP)} = g.{nameof(GROUPS.ID_GROUP)} " +
                $"JOIN {nameof(USERS)} u1 ON gi.{nameof(GROUP_INVITE.IDAUTHOR)} = u1.{nameof(USERS.ID_USER)} " +
                $"JOIN {nameof(USERS)} u2 ON gi.{nameof(GROUP_INVITE.IDUSER)} = u2.{nameof(USERS.ID_USER)} ";

        public async Task<List<GetGroupInviteResponse?>> GetGroupInviteByIdUserAsync(GetGroupInvitePaginationRequest getGroupInvitePaginationRequest)
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                await _dbConnection.OpenAsync();
            }
            try
            {
                DynamicParameters dynamicParameters = new();
                string WHERE = "1=1 ";

                if (getGroupInvitePaginationRequest.IdGroup is not null)
                {
                    WHERE += $"AND gi.{nameof(GROUP_INVITE.IDGROUP)} = @GroupId ";
                    dynamicParameters.Add("@GroupId", getGroupInvitePaginationRequest.IdGroup);
                }
                if (getGroupInvitePaginationRequest.IdUser is not null)
                {
                    WHERE += $"AND gi.{nameof(GROUP_INVITE.IDUSER)} = @UserId ";
                    dynamicParameters.Add("@UserId", getGroupInvitePaginationRequest.IdUser);
                }
                var query = new QueryBuilder<GetGroupInviteResponse>()
                    .Select(SELECT)
                    .From(FROM)
                    .Where(WHERE)
                    .OrderBy(getGroupInvitePaginationRequest)
                    .Limit(getGroupInvitePaginationRequest);
                return (await _dbConnection.QueryAsync<GetGroupInviteResponse?>(query.Build(), dynamicParameters, _fbTransaction)).AsList();
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
