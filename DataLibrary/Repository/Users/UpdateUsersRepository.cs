using System.Data;
using Dapper;
using DataLibrary.Entities;
using DataLibrary.Helper;
using DataLibrary.IRepository.Users;
using DataLibrary.Model.DTO.Request.TableRequest;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository.Users
{
    public class UpdateUsersRepository(FbConnection dbConnection, FbTransaction? fbTransaction) : IUpdateUsersRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;
        private readonly FbTransaction? _fbTransaction = fbTransaction;


        public async Task UpdateUserAsync(USERS user)
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                await _dbConnection.OpenAsync();
            }
            try
            {
                var updateBuilder = new QueryBuilder<USERS>()
                    .Update("USERS ", user)
                    .Where("ID_USER = @ID_USER");
                string updateQuery = updateBuilder.Build();
                await _dbConnection.ExecuteAsync(updateQuery, user, _fbTransaction);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

        public async Task UpdateColumnUserAsync(GetUpdateUserRequest getUpdateUserRequest, int userId, string? salt)
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                await _dbConnection.OpenAsync();
            }
            try
            {
                DynamicParameters dynamicParameters = new();
                var updateBuilder = new QueryBuilder<GetUpdateUserRequest>()
                    .UpdateColumns("USERS", getUpdateUserRequest.Column)
                    .Where("ID_USER = @UserId");
                string updateQuery = updateBuilder.Build();
                dynamicParameters.Add("@UserId", userId);

                foreach (string column in getUpdateUserRequest.Column)
                {
                    switch (column)
                    {
                        case "USER_PASSWORD":
                            {
                                string password = getUpdateUserRequest.USER_PASSWORD ?? throw new Exception("Password is null");
                                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, salt);
                                dynamicParameters.Add($"@{column}", hashedPassword);
                            }
                            break;
                        case "LOGIN":
                            {
                                string login = getUpdateUserRequest.LOGIN ?? throw new Exception("Login is null");
                                dynamicParameters.Add($"@{column}", login);
                            }
                            break;
                        case "EMAIL":
                            {
                                string email = getUpdateUserRequest.EMAIL ?? throw new Exception("Email is null");
                                dynamicParameters.Add($"@{column}", email);
                            }
                            break;
                        case "FIRSTNAME":
                            {
                                string firstname = getUpdateUserRequest.FIRSTNAME ?? throw new Exception("Firstname is null");
                                dynamicParameters.Add($"@{column}", firstname);
                            }
                            break;
                        case "SURNAME":
                            {
                                string surname = getUpdateUserRequest.SURNAME ?? throw new Exception("Surname is null");
                                dynamicParameters.Add($"@{column}", surname);
                            }
                            break;
                        case "PHONE_NUMBER":
                            {
                                int phoneNumber = getUpdateUserRequest.PHONE_NUMBER ?? throw new Exception("Phone Number is null");
                                dynamicParameters.Add($"@{column}", phoneNumber);
                            }
                            break;
                        case "AVATAR":
                            {
                                byte[] avatar = getUpdateUserRequest.AVATAR ?? throw new Exception("Avatar is null");
                                dynamicParameters.Add($"@{column}", avatar);
                            }
                            break;
                        case "GROUP_COUNTER":
                            {
                                int groupCounter = getUpdateUserRequest.GROUP_COUNTER ?? throw new Exception("Group Counter is null");
                                dynamicParameters.Add($"@{column}", groupCounter);
                            }
                            break;
                    }

                }
                await _dbConnection.ExecuteAsync(updateQuery, dynamicParameters, _fbTransaction);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}