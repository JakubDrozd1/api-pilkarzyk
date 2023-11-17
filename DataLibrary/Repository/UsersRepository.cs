using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLibrary.Entities;
using DataLibrary.IRepository;
using Dapper;
using System.Data.SqlClient;
using System.Data;

namespace DataLibrary.Repository
{
    public class UsersRepository(SqlConnection dbConnection) : IUsersRepository
    {
        private readonly SqlConnection _dbConnection = dbConnection;
        public async Task<List<User>> GetAllUsersAsync()
        {
            using SqlConnection db = _dbConnection;
            await db.OpenAsync();
            return (await db.QueryAsync<User>("SELECT * FROM Users")).AsList();
        }

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            using SqlConnection db = _dbConnection;
            await db.OpenAsync();
            return await db.QueryFirstOrDefaultAsync<User>("SELECT * FROM Users WHERE IdUser = @UserId", new { UserId = userId });
        }

        public async Task AddUserAsync(User user)
        {
            try
            {
                using (SqlConnection db = _dbConnection)
                {
                    await db.OpenAsync();
                    await db.ExecuteAsync("INSERT INTO Users (Login, Password, Email, FirstName, LastName, PhoneNumber) VALUES (@Login, @Password, @Email, @FirstName, @LastName, @PhoneNumber)", user);
                }
            }
            catch (Exception ex)
            {
                // Obsłuż wyjątek lub wyświetl komunikat o błędzie
                Console.WriteLine($"Wystąpił błąd: {ex.Message}");
            }
        }

        public async Task UpdateUserAsync(User user)
        {
            using SqlConnection db = _dbConnection;
            await db.OpenAsync();
            await db.ExecuteAsync("UPDATE Users SET Login = @Login, Password = @Password, Email = @Email, FirstName = @FirstName, LastName = @LastName, PhoneNumber = @PhoneNumber WHERE IdUser = @IdUser", user);
        }

        public async Task DeleteUserAsync(int userId)
        {
            using SqlConnection db = _dbConnection;
            await db.OpenAsync();
            await db.ExecuteAsync("DELETE FROM Users WHERE IdUser = @UserId", new { UserId = userId });
        }
    }
}
