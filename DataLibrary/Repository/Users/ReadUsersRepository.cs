using System.Data;
using Dapper;
using DataLibrary.Entities;
using DataLibrary.IRepository;
using DataLibrary.Model.DTO.Request;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository
{
    public class ReadUsersRepository(FbConnection dbConnection) : IReadUsersRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;

        public async Task<List<USERS>> GetAllUsersAsync(GetUsersPaginationRequest getUsersPaginationRequest, FbTransaction? transaction = null)
        {

            var query = new QueryBuilder<USERS>()
                .Select("* ")
                .From("USERS ")
                .OrderBy(getUsersPaginationRequest)
                .Limit(getUsersPaginationRequest);
            FbConnection db = transaction?.Connection ?? _dbConnection;

            if (transaction == null && db.State != ConnectionState.Open)
            {
                await db.OpenAsync();
            }

            return (await db.QueryAsync<USERS>(query.Build(), transaction)).AsList();

        }

        public async Task<USERS?> GetUserByIdAsync(int userId, FbTransaction? transaction = null)
        {

            var query = new QueryBuilder<USERS>()
                .Select("* ")
                .From("USERS ")
                .Where("ID_USER = @UserId ");
            FbConnection db = transaction?.Connection ?? _dbConnection;

            if (transaction == null && db.State != ConnectionState.Open)
            {
                await db.OpenAsync();
            }

            return await db.QuerySingleOrDefaultAsync<USERS>(query.Build(), new { UserId = userId }, transaction);

        }

        public async Task<USERS?> GetUserByLoginAsync(string? login, FbTransaction? transaction = null)
        {
            var query = new QueryBuilder<USERS>()
                .Select("* ")
                .From("USERS ")
                .Where("LOGIN = @Login ");
            FbConnection db = transaction?.Connection ?? _dbConnection;

            if (transaction == null && db.State != ConnectionState.Open)
            {
                await db.OpenAsync();
            }

            return await db.QuerySingleOrDefaultAsync<USERS>(query.Build(), new { Login = login }, transaction);
        }

        public async Task<USERS?> GetUserByLoginAndPasswordAsync(GetUsersByLoginAndPassword getUsersByLoginAndPassword, FbTransaction? transaction = null)
        {
            FbConnection db = transaction?.Connection ?? _dbConnection;

            if (transaction == null && db.State != ConnectionState.Open)
            {
                await db.OpenAsync();
            }

            FbTransaction localTransaction = transaction ?? await db.BeginTransactionAsync();

            try
            {
                DynamicParameters dynamicParameters = new();
                string WHERE = "1=1 ";
                USERS? user = null;

                if (getUsersByLoginAndPassword.Login is not null)
                {
                    user = await GetUserByLoginAsync(getUsersByLoginAndPassword.Login, localTransaction) ?? throw new InvalidOperationException("User is null");
                    WHERE += $"AND {nameof(USERS.LOGIN)} = @Login ";
                    dynamicParameters.Add("@Login", getUsersByLoginAndPassword.Login);
                }

                if (getUsersByLoginAndPassword.Password is not null)
                {
                    if (!BCrypt.Net.BCrypt.Verify(getUsersByLoginAndPassword.Password, user?.PASSWORD))
                        throw new Exception("Password is not correct");
                    WHERE += $"AND {nameof(USERS.PASSWORD)} = @Password ";
                    dynamicParameters.Add("@Password", user?.PASSWORD);
                }

                var query = new QueryBuilder<USERS>()
                    .Select("* ")
                    .From("USERS ")
                    .Where(WHERE);

                return await db.QuerySingleOrDefaultAsync<USERS>(query.Build(), dynamicParameters, localTransaction);
            }
            catch (Exception ex)
            {
                localTransaction?.Rollback();
                throw new Exception($"Error while executing query: {ex.Message}");
            }
        }
    }
}
