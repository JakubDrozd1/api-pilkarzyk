

using System.Data;
using Dapper;
using DataLibrary.Entities;
using DataLibrary.Helper;
using DataLibrary.IRepository.ResetPassword;
using DataLibrary.Model.DTO.Request.Pagination;
using DataLibrary.Model.DTO.Response;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository.ResetPassword
{
    internal class ReadResetPasswordRepository(FbConnection dbConnection, FbTransaction? fbTransaction) : IReadResetPasswordRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;
        private readonly FbTransaction? _fbTransaction = fbTransaction;
        private static readonly string SELECT =
            $"u.{nameof(USERS.LOGIN)}, " +
            $"u.{nameof(USERS.FIRSTNAME)}, " +
            $"u.{nameof(USERS.SURNAME)}, " +
            $"u.{nameof(USERS.EMAIL)}, " +
            $"rp.{nameof(RESET_PASSWORD.IDUSER)}, " +
            $"rp.{nameof(RESET_PASSWORD.DATE_ADD)} AS DateAdd, " +
            $"rp.{nameof(RESET_PASSWORD.ID_RESET_PASSWORD)} AS IdResetPassword ";
        private static readonly string FROM
              = $"{nameof(RESET_PASSWORD)} rp " +
                $"JOIN {nameof(USERS)} u ON rp.{nameof(RESET_PASSWORD.IDUSER)} = u.{nameof(USERS.ID_USER)} ";

        public async Task<RESET_PASSWORD?> GetLastAdded(int userId)
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
                var query = new QueryBuilder<RESET_PASSWORD>()
                    .Select("* ")
                    .From($"{nameof(RESET_PASSWORD)} ")
                    .Where("IDUSER = @UserId ")
                    .OrderBy(pagination)
                    .Limit(pagination);
                return await _dbConnection.QuerySingleOrDefaultAsync<RESET_PASSWORD>(query.Build(), new { UserId = userId }, _fbTransaction);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

        public async Task<GetResetPasswordResponse?> GetResetPasswordByIdAsync(int passwordResetId)
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                await _dbConnection.OpenAsync();
            }
            try
            {
                var query = new QueryBuilder<GetResetPasswordResponse>()
                    .Select(SELECT)
                    .From(FROM)
                    .Where("rp.ID_RESET_PASSWORD = @ResetPasswordId ");
                return await _dbConnection.QuerySingleOrDefaultAsync<GetResetPasswordResponse>(query.Build(), new { ResetPasswordId = passwordResetId }, _fbTransaction);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
