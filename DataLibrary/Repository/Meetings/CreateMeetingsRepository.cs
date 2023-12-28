using System.Data;
using Dapper;
using DataLibrary.Helper;
using DataLibrary.IRepository.Meetings;
using DataLibrary.Model.DTO.Request;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository.Meetings
{
    public class CreateMeetingsRepository(FbConnection dbConnection, FbTransaction? fbTransaction) : ICreateMeetingsRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;
        private readonly FbTransaction? _fbTransaction = fbTransaction;

        public async Task AddMeetingAsync(GetMeetingRequest getMeetingRequest)
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                await _dbConnection.OpenAsync();
            }
            try
            {
                var insertBuilder = new QueryBuilder<GetMeetingRequest>()
                    .Insert("MEETINGS ", getMeetingRequest);
                string insertQuery = insertBuilder.Build();
                await _dbConnection.ExecuteAsync(insertQuery, getMeetingRequest, _fbTransaction);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
