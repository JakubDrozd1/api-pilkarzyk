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
                    if (!BCrypt.Net.BCrypt.Verify(getUsersByLoginAndPassword.Password, user?.USER_PASSWORD))
                        throw new Exception("Password is not correct");
                    WHERE += $"AND {nameof(USERS.USER_PASSWORD)} = @Password ";
                    dynamicParameters.Add("@Password", user?.USER_PASSWORD);
                }

                var query = new QueryBuilder<USERS>()
                    .Select("* ")
                    .From("USERS ")
                    .Where(WHERE);

                return await db.QuerySingleOrDefaultAsync<USERS>(query.Build(), dynamicParameters, localTransaction);
            }
            catch (Exception ex)
            {
                localTransaction?.RollbackAsync();
                throw new Exception($"Error while executing query: {ex.Message}");
            }
        }

        public async Task<List<USERS>> GetAllUsersWithoutGroupAsync(GetUsersWithoutGroupPaginationRequest getUsersWithoutGroupPaginationRequest, FbTransaction? transaction = null)
        {
            FbConnection db = transaction?.Connection ?? _dbConnection;

            if (transaction == null && db.State != ConnectionState.Open)
            {
                await db.OpenAsync();
            }

            FbTransaction localTransaction = transaction ?? await db.BeginTransactionAsync();
            ReadGroupsUsersRepository readGroupsUsersRepository = new(db);
            GetUsersGroupsPaginationRequest getUsersGroupsPaginationRequest = new()
            {
                IdGroup = getUsersWithoutGroupPaginationRequest.IdGroup,
                Page = getUsersWithoutGroupPaginationRequest.Page,
                OnPage = getUsersWithoutGroupPaginationRequest.OnPage,
            };
            var usersTemp = await readGroupsUsersRepository.GetListGroupsUserAsync(getUsersGroupsPaginationRequest, localTransaction);
            DynamicParameters dynamicParameters = new();
            string WHERE = "1=1 ";

            foreach (var item in usersTemp)
            {
                WHERE += $"AND u.{nameof(USERS.ID_USER)} <> @UserId{item.IdUser} ";
                dynamicParameters.Add($"@UserId{item.IdUser}", item.IdUser);
            }

            var query = new QueryBuilder<USERS>()
                .Select($"DISTINCT u.{nameof(USERS.ID_USER)}, " +
                $"u.{nameof(USERS.LOGIN)}, " +
                $"u.{nameof(USERS.USER_PASSWORD)}, " +
                $"u.{nameof(USERS.EMAIL)}, " +
                $"u.{nameof(USERS.FIRSTNAME)}, " +
                $"u.{nameof(USERS.SURNAME)}, " +
                $"u.{nameof(USERS.PHONE_NUMBER)}, " +
                $"u.{nameof(USERS.IS_ADMIN)}, " +
                $"u.{nameof(USERS.SALT)} ")
                .From($"{nameof(USERS)} u " +
                $"LEFT JOIN {nameof(GROUPS_USERS)} gu ON u.{nameof(USERS.ID_USER)} = gu.{nameof(GROUPS_USERS.IDUSER)} ")
                .Where(WHERE)
                .OrderBy(getUsersWithoutGroupPaginationRequest)
                .Limit(getUsersWithoutGroupPaginationRequest);

            return (await db.QueryAsync<USERS>(query.Build(), dynamicParameters, localTransaction)).AsList();

        }

        public async Task<USERS?> GetUserByEmailAsync(string email, FbTransaction? transaction = null)
        {
            var query = new QueryBuilder<USERS>()
                .Select("* ")
                .From("USERS ")
                .Where("Email = @Email ");
            FbConnection db = transaction?.Connection ?? _dbConnection;

            if (transaction == null && db.State != ConnectionState.Open)
            {
                await db.OpenAsync();
            }

            return await db.QuerySingleOrDefaultAsync<USERS>(query.Build(), new { Email = email }, transaction);
        }

        public async Task<USERS?> GetUserByPhoneNumberAsync(int phoneNumber, FbTransaction? transaction = null)
        {
            var query = new QueryBuilder<USERS>()
                .Select("* ")
                .From("USERS ")
                .Where("PHONE_NUMBER = @PhoneNumber ");
            FbConnection db = transaction?.Connection ?? _dbConnection;

            if (transaction == null && db.State != ConnectionState.Open)
            {
                await db.OpenAsync();
            }

            return await db.QuerySingleOrDefaultAsync<USERS>(query.Build(), new { PhoneNumber = phoneNumber }, transaction);
        }

        public async Task<string?> GetSaltByUserId(int userId, FbTransaction? transaction = null)
        {
            var query = new QueryBuilder<USERS>()
                .Select("* ")
                .From("USERS ")
                .Where("ID_USER = @IdUser ");
            FbConnection db = transaction?.Connection ?? _dbConnection;

            if (transaction == null && db.State != ConnectionState.Open)
            {
                await db.OpenAsync();
            }

            USERS? user = await db.QuerySingleOrDefaultAsync<USERS>(query.Build(), new { IdUser = userId }, transaction);
            return user == null ? throw new Exception("Salt is null") : user.SALT;
        }
    }
}
