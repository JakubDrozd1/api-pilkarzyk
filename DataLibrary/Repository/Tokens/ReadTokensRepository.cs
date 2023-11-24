using Dapper;
using System.Data;
using DataLibrary.Entities;
using DataLibrary.IRepository;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository
{
    public class ReadTokensRepository(FbConnection dbConnection) : IReadTokensRepository
    {
        private readonly FbConnection dbConnection = dbConnection;
        public async Task<TOKENS?> GetTokenByUserAsync(int userId, FbTransaction? transaction = null)
        {

            var query = new QueryBuilder<TOKENS>()
                .Select("* ")
                .From("TOKENS ")
                .Where("IDUser = @UserId ");
            FbConnection db = transaction?.Connection ?? dbConnection;

            if (transaction == null && db.State != ConnectionState.Open)
            {
                await db.OpenAsync();
            }

            return await db.QuerySingleOrDefaultAsync<TOKENS>(query.Build(), new { UserId = userId }, transaction);
        }
    }
}
