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
                .Select("*")
                .From("USERS")
                .Where("ID_USER = @UserId");
            FbConnection db = transaction?.Connection ?? _dbConnection;

            if (transaction == null && db.State != ConnectionState.Open)
            {
                await db.OpenAsync();
            }

            return await db.QuerySingleOrDefaultAsync<USERS>(query.Build(), new { UserId = userId }, transaction);

        }
    }
}
