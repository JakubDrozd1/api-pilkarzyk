using Dapper;
using DataLibrary.Entities;
using DataLibrary.Helper;
using System.Data;
using DataLibrary.IRepository.GroupInvite;
using DataLibrary.Model.DTO.Response;
using FirebirdSql.Data.FirebirdClient;
using DataLibrary.Model.DTO.Request.Pagination;
using DataLibrary.Model.DTO.Request.TableRequest;

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
                $"gi.{nameof(GROUP_INVITE.EMAIL)}, " +
                $"u1.{nameof(USERS.FIRSTNAME)} AS FirstnameAuthor, " +
                $"u1.{nameof(USERS.SURNAME)} AS SurnameAuthor, " +
                $"u2.{nameof(USERS.FIRSTNAME)}, " +
                $"u2.{nameof(USERS.PHONE_NUMBER)} AS PhoneNumber, " +
                $"u2.{nameof(USERS.SURNAME)} ";
        private static readonly string FROM
              = $"{nameof(GROUP_INVITE)} gi " +
                $"JOIN {nameof(GROUPS)} g ON gi.{nameof(GROUP_INVITE.IDGROUP)} = g.{nameof(GROUPS.ID_GROUP)} " +
                $"LEFT JOIN {nameof(USERS)} u1 ON gi.{nameof(GROUP_INVITE.IDAUTHOR)} = u1.{nameof(USERS.ID_USER)} " +
                $"LEFT JOIN {nameof(USERS)} u2 ON gi.{nameof(GROUP_INVITE.IDUSER)} = u2.{nameof(USERS.ID_USER)} ";

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
                if (getGroupInvitePaginationRequest.Email is not null)
                {
                    WHERE += $"AND gi.{nameof(GROUP_INVITE.EMAIL)} = @Email ";
                    dynamicParameters.Add("@Email", getGroupInvitePaginationRequest.Email);
                }
                if (getGroupInvitePaginationRequest.DateFrom is not null)
                {
                    WHERE += $"AND m.{nameof(GROUP_INVITE.DATE_ADD)} >= @DateFrom ";
                    dynamicParameters.Add("@DateFrom", getGroupInvitePaginationRequest.DateFrom);
                }
                if (getGroupInvitePaginationRequest.DateTo is not null)
                {
                    WHERE += $"AND m.{nameof(GROUP_INVITE.DATE_ADD)} <= @DateTo ";
                    dynamicParameters.Add("@DateTo", getGroupInvitePaginationRequest.DateTo);
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

        public async Task<GROUP_INVITE?> GetLastAddedInvite(GetGroupInviteRequest getGroupInviteRequest)
        {
            var pagination = new Pagination()
            {
                SortColumn = "DATE_ADD",
                SortMode = "DESC",
                OnPage = 1,
                Page = 0
            };
            if (_dbConnection.State != ConnectionState.Open)
            {
                await _dbConnection.OpenAsync();
            }
            try
            {
                DynamicParameters dynamicParameters = new();
                string WHERE = "1=1 ";
                if (getGroupInviteRequest.IDUSER is not null)
                {

                    WHERE += $"AND {nameof(GROUP_INVITE.IDUSER)} = @UserId ";
                    dynamicParameters.Add("@UserId", getGroupInviteRequest.IDUSER);
                }

                if (getGroupInviteRequest.EMAIL is not null)
                {
                    WHERE += $"AND {nameof(GROUP_INVITE.EMAIL)} = @Email ";
                    dynamicParameters.Add("@Email", getGroupInviteRequest.EMAIL);
                }

                WHERE += $"AND {nameof(GROUP_INVITE.IDAUTHOR)} = @AuthorId ";
                dynamicParameters.Add("@AuthorId", getGroupInviteRequest.IDAUTHOR);

                WHERE += $"AND {nameof(GROUP_INVITE.IDGROUP)} = @GroupId ";
                dynamicParameters.Add("@GroupId", getGroupInviteRequest.IDGROUP);

                var query = new QueryBuilder<GROUP_INVITE>()
                    .Select("* ")
                    .From($"{nameof(GROUP_INVITE)} ")
                    .Where(WHERE)
                    .OrderBy(pagination)
                    .Limit(pagination);
                return await _dbConnection.QuerySingleOrDefaultAsync<GROUP_INVITE>(query.Build(), dynamicParameters, _fbTransaction);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

        public async Task<GROUP_INVITE?> GetGroupInviteByIdAsync(int groupInviteId)
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                await _dbConnection.OpenAsync();
            }
            try
            {
                var query = new QueryBuilder<GROUP_INVITE>()
                    .Select("* ")
                    .From($"{nameof(GROUP_INVITE)} ")
                    .Where("ID_GROUP_INVITE = @GroupInviteId ");
                return await _dbConnection.QuerySingleOrDefaultAsync<GROUP_INVITE>(query.Build(), new { GroupInviteId = groupInviteId }, _fbTransaction);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
