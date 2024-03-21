using Dapper;
using System.Data;
using DataLibrary.Entities;
using DataLibrary.Helper;
using DataLibrary.IRepository.Guests;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository.Guests
{
    public class UpdateGuestsRepository(FbConnection dbConnection, FbTransaction? fbTransaction) : IUpdateGuestsRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;
        private readonly FbTransaction? _fbTransaction = fbTransaction;
        public async Task UpdateGuestsAsync(GUESTS guest)
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                await _dbConnection.OpenAsync();
            }
            try
            {
                var updateBuilder = new QueryBuilder<GUESTS>()
                    .Update("GUESTS ", guest)
                    .Where("ID_GUEST = @ID_GUEST ");
                string updateQuery = updateBuilder.Build();
                await _dbConnection.ExecuteAsync(updateQuery, guest, _fbTransaction);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
