using Dapper;
using System.Data;
using DataLibrary.Entities;
using FirebirdSql.Data.FirebirdClient;
using DataLibrary.IRepository.EmailSender;

namespace DataLibrary.Repository.EmailSender
{
    public class ReadEmailSenderRepository(FbConnection dbConnection) : IReadEmailSender
    {
        private readonly FbConnection _dbConnection = dbConnection;
        public async Task<EMAIL_SENDER?> GetEmailDetailsAsync(string email, FbTransaction? transaction = null)
        {
            var query = new QueryBuilder<EMAIL_SENDER>()
                .Select("* ")
                .From("EMAIL_SENDER ")
                .Where("EMAIL = @Email ");
            FbConnection db = transaction?.Connection ?? _dbConnection;

            if (transaction == null && db.State != ConnectionState.Open)
            {
                await db.OpenAsync();
            }

            return await db.QuerySingleOrDefaultAsync<EMAIL_SENDER>(query.Build(), new { Email = email }, transaction);
        }
    }
}
