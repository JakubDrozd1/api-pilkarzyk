using Dapper;
using DataLibrary.Helper;
using System.Data;
using DataLibrary.IRepository.Guests;
using DataLibrary.Model.DTO.Request.TableRequest;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository.Guests
{
    public class CreateGuestsRepository(FbConnection dbConnection, FbTransaction? fbTransaction) : ICreateGuestsRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;
        private readonly FbTransaction? _fbTransaction = fbTransaction;
        public async Task AddGuestsAsync(GetGuestRequest getGuestRequest)
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                await _dbConnection.OpenAsync();
            }
            try
            {
                var insertBuilder = new QueryBuilder<GetGuestRequest>().Insert("GUESTS ", getGuestRequest);
                string insertQuery = insertBuilder.Build();
                await _dbConnection.ExecuteAsync(insertQuery, getGuestRequest, _fbTransaction);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
