
using Dapper;
using DataLibrary.Entities;
using DataLibrary.Helper;
using System.Data;
using FirebirdSql.Data.FirebirdClient;
using DataLibrary.IRepository.Guests;

namespace DataLibrary.Repository.Guests
{
    public class DeleteGuestsRepository(FbConnection dbConnection, FbTransaction? fbTransaction) : IDeleteGuestsRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;
        private readonly FbTransaction? _fbTransaction = fbTransaction;

        public async Task DeleteGuestsAsync(int guestsId)
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                await _dbConnection.OpenAsync();
            }
            try
            {
                var deleteBuilder = new QueryBuilder<GUESTS>()
                    .Delete("GUESTS ")
                    .Where("ID_GUEST = @GuestsId ");
                string deleteQuery = deleteBuilder.Build();
                await _dbConnection.ExecuteAsync(deleteQuery, new { GuestsId = guestsId }, _fbTransaction);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
