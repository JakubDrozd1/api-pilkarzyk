using System.Data;
using Dapper;
using DataLibrary.Entities;
using DataLibrary.Helper;
using DataLibrary.IRepository.Users;
using DataLibrary.Model.DTO.Request;
using DataLibrary.Model.DTO.Request.Pagination;
using DataLibrary.Model.DTO.Response;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository.Users
{
    public class ReadUsersRepository(FbConnection dbConnection, FbTransaction? fbTransaction) : IReadUsersRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;
        private readonly FbTransaction? _fbTransaction = fbTransaction;
        private string SELECT = " ID_USER, LOGIN, USER_PASSWORD, FIRSTNAME, SURNAME, EMAIL, PHONE_NUMBER, SALT, IS_ADMIN, GROUP_COUNTER ";
        public async Task<List<USERS>> GetAllUsersAsync(GetUsersPaginationRequest getUsersPaginationRequest)
        {

            if (_dbConnection.State != ConnectionState.Open)
            {
                await _dbConnection.OpenAsync();
            }
            try
            {
                if (getUsersPaginationRequest.IsAvatar)
                {
                    SELECT += ", AVATAR ";
                }
                var query = new QueryBuilder<USERS>()
                    .Select(SELECT)
                    .From("USERS ")
                    .OrderBy(getUsersPaginationRequest)
                    .Limit(getUsersPaginationRequest);
                return (await _dbConnection.QueryAsync<USERS>(query.Build(), _fbTransaction)).AsList();
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

        public async Task<USERS?> GetUserByIdAsync(int userId)
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                await _dbConnection.OpenAsync();
            }
            try
            {
                var query = new QueryBuilder<USERS>()
                    .Select("* ")
                    .From("USERS ")
                    .Where("ID_USER = @UserId ");
                return await _dbConnection.QuerySingleOrDefaultAsync<USERS>(query.Build(), new { UserId = userId }, _fbTransaction);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

        public async Task<USERS?> GetUserByLoginAsync(string? login)
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                await _dbConnection.OpenAsync();
            }
            try
            {
                var query = new QueryBuilder<USERS>()
                    .Select("* ")
                    .From("USERS ")
                    .Where("LOGIN = @Login ");
                return await _dbConnection.QuerySingleOrDefaultAsync<USERS>(query.Build(), new { Login = login }, _fbTransaction);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

        public async Task<USERS?> GetUserByLoginAndPasswordAsync(GetUsersByLoginAndPasswordRequest getUsersByLoginAndPassword, USERS? user)
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                await _dbConnection.OpenAsync();
            }
            try
            {
                DynamicParameters dynamicParameters = new();
                string WHERE = "1=1 ";
                if (getUsersByLoginAndPassword.Login is not null)
                {
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
                return await _dbConnection.QuerySingleOrDefaultAsync<USERS>(query.Build(), dynamicParameters, _fbTransaction);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

        public async Task<List<USERS>> GetAllUsersWithoutGroupAsync(GetUsersWithoutGroupPaginationRequest getUsersWithoutGroupPaginationRequest, List<GetGroupsUsersResponse> getGroupsUsersResponse)
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                await _dbConnection.OpenAsync();
            }
            try
            {
                DynamicParameters dynamicParameters = new();
                string WHERE = "1=1 ";

                foreach (var item in getGroupsUsersResponse)
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

                return (await _dbConnection.QueryAsync<USERS>(query.Build(), dynamicParameters, _fbTransaction)).AsList();
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

        public async Task<USERS?> GetUserByEmailAsync(string email)
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                await _dbConnection.OpenAsync();
            }
            try
            {
                var query = new QueryBuilder<USERS>()
                    .Select("* ")
                    .From("USERS ")
                    .Where("Email = @Email ");
                return await _dbConnection.QuerySingleOrDefaultAsync<USERS>(query.Build(), new { Email = email }, _fbTransaction);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

        public async Task<USERS?> GetUserByPhoneNumberAsync(int phoneNumber)
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                await _dbConnection.OpenAsync();
            }
            try
            {
                var query = new QueryBuilder<USERS>()
                    .Select("* ")
                    .From("USERS ")
                    .Where("PHONE_NUMBER = @PhoneNumber ");
                return await _dbConnection.QuerySingleOrDefaultAsync<USERS>(query.Build(), new { PhoneNumber = phoneNumber }, _fbTransaction);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

        public async Task<string?> GetSaltByUserId(int userId)
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                await _dbConnection.OpenAsync();
            }
            try
            {
                var query = new QueryBuilder<USERS>()
                    .Select("* ")
                    .From("USERS ")
                    .Where("ID_USER = @IdUser ");
                USERS? user = await _dbConnection.QuerySingleOrDefaultAsync<USERS>(query.Build(), new { IdUser = userId }, _fbTransaction);
                return user == null ? throw new Exception("Salt is null") : user.SALT;
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
