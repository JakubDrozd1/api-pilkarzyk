using Dapper;
using DataLibrary.Helper;
using System.Data;
using DataLibrary.IRepository.ResetPassword;
using DataLibrary.Model.DTO.Request;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository.ResetPassword
{
    public class CreateResetPasswordRepository(FbConnection dbConnection, FbTransaction? fbTransaction) : ICreateResetPasswordRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;
        private readonly FbTransaction? _fbTransaction = fbTransaction;
        public async Task AddResetPasswordAsync(GetResetPasswordRequest getResetPasswordRequest)
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                await _dbConnection.OpenAsync();
            }
            try
            {
                var insertBuilder = new QueryBuilder<GetResetPasswordRequest>()
                    .Insert("RESET_PASSWORD ", getResetPasswordRequest);
                string insertQuery = insertBuilder.Build();
                await _dbConnection.ExecuteAsync(insertQuery, getResetPasswordRequest, _fbTransaction);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
