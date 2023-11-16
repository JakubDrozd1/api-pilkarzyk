using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLibrary.Entities;
using DataLibrary.IRepository;
using Dapper;
using System.Data.SqlClient;

namespace DataLibrary.Repository
{
    internal class UsersRepository : IUsersRepository
    {
        private readonly string connectionString;

        public UsersRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<User> GetAllUsers()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return connection.Query<User>("SELECT * FROM Users").ToList();
            }
        }

        public User? GetUserById(int userId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return connection.QueryFirstOrDefault<User>("SELECT * FROM Users WHERE IdUser = @UserId", new { UserId = userId });
            }
        }

        public void AddUser(User user)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                connection.Execute("INSERT INTO Users (Login, Password, Email, FirstName, LastName, PhoneNumber) " +
                                   "VALUES (@Login, @Password, @Email, @FirstName, @LastName, @PhoneNumber)", user);
            }
        }

        public void UpdateUser(User user)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                connection.Execute("UPDATE Users SET Login = @Login, Password = @Password, Email = @Email, " +
                                   "FirstName = @FirstName, LastName = @LastName, PhoneNumber = @PhoneNumber " +
                                   "WHERE IdUser = @IdUser", user);
            }
        }

        public void DeleteUser(int userId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                connection.Execute("DELETE FROM Users WHERE IdUser = @UserId", new { UserId = userId });
            }
        }
    }
}
