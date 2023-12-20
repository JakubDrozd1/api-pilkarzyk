using System.Data;
using Dapper;
using DataLibrary.Entities;
using DataLibrary.Helper;
using DataLibrary.IRepository.Users;
using DataLibrary.Model.DTO.Request;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository.Users
{
    public class UpdateUsersRepository(FbConnection dbConnection) : IUpdateUsersRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;

        public async Task UpdateUserAsync(USERS user, FbTransaction? transaction = null)
        {
            var updateBuilder = new QueryBuilder<USERS>()
                .Update("USERS ", user)
                .Where("ID_USER = @ID_USER");
            string updateQuery = updateBuilder.Build();
            FbConnection db = transaction?.Connection ?? _dbConnection;
            if (transaction == null && db.State != ConnectionState.Open)
            {
                await db.OpenAsync();
            }
            await db.ExecuteAsync(updateQuery, user, transaction);
        }

        public async Task UpdateColumnUserAsync(GetUpdateUserRequest getUpdateUserRequest, int userId, FbTransaction? transaction = null)
        {
            DynamicParameters dynamicParameters = new();
            FbConnection db = transaction?.Connection ?? _dbConnection;
            ReadUsersRepository readUsersRepository = new(db);

            if (transaction == null && db.State != ConnectionState.Open)
            {
                await db.OpenAsync();
            }
            var localTransaction = transaction ?? await db.BeginTransactionAsync();
            try
            {
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
                                string salt = await readUsersRepository.GetSaltByUserId(userId, localTransaction) ?? throw new Exception("Salt is null");
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
                    }

                }
                await db.ExecuteAsync(updateQuery, dynamicParameters, localTransaction);

                if (transaction == null)
                {
                    await localTransaction.CommitAsync();
                }
            }
            catch (Exception ex)
            {
                localTransaction?.RollbackAsync();
                throw new Exception($"{ex.Message}");
            }
        }
    }
}