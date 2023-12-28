using Dapper;
using System.Data;
using DataLibrary.Entities;
using FirebirdSql.Data.FirebirdClient;
using DataLibrary.IRepository.EmailSender;
using DataLibrary.Helper;

namespace DataLibrary.Repository.EmailSender
{
    public class ReadEmailSenderRepository(FbConnection dbConnection, FbTransaction? fbTransaction) : IReadEmailSender
    {
        private readonly FbConnection _dbConnection = dbConnection;
        private readonly FbTransaction? _fbTransaction = fbTransaction;
        public async Task<EMAIL_SENDER?> GetEmailDetailsAsync(string email)
        {

            if (_dbConnection.State != ConnectionState.Open)
            {
                await _dbConnection.OpenAsync();
            }
            try
            {
                var query = new QueryBuilder<EMAIL_SENDER>()
                    .Select("* ")
                    .From("EMAIL_SENDER ")
                    .Where("EMAIL = @Email ");
                return await _dbConnection.QuerySingleOrDefaultAsync<EMAIL_SENDER>(query.Build(), new { Email = email }, _fbTransaction);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
