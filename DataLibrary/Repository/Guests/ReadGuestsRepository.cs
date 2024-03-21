using Dapper;
using System.Data;
using DataLibrary.Entities;
using DataLibrary.Helper;
using DataLibrary.IRepository.Guests;
using FirebirdSql.Data.FirebirdClient;
using DataLibrary.Model.DTO.Response;

namespace DataLibrary.Repository.Guests
{
    public class ReadGuestsRepository(FbConnection dbConnection, FbTransaction? fbTransaction) : IReadGuestsRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;
        private readonly FbTransaction? _fbTransaction = fbTransaction;
        public async Task<List<GUESTS?>> GetAllGuestFromMeetingAsync(int meetingId)
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                await _dbConnection.OpenAsync();
            }
            try
            {
                var query = new QueryBuilder<GUESTS>()
                    .Select("* ")
                    .From($"{nameof(GUESTS)} ")
                    .Where("IDMEETING = @MeetingId ");
                return (await _dbConnection.QueryAsync<GUESTS?>(query.Build(), new { MeetingId = meetingId }, _fbTransaction)).AsList();
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

        public async Task<GUESTS?> GetGuestByIdAsync(int guestId)
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                await _dbConnection.OpenAsync();
            }
            try
            {
                var query = new QueryBuilder<GetMeetingGroupsResponse>()
                    .Select("* ")
                    .From($"{nameof(GUESTS)} ")
                    .Where("ID_GUEST = @GuestId ");
                return await _dbConnection.QuerySingleOrDefaultAsync<GUESTS>(query.Build(), new { GuestId = guestId }, _fbTransaction);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
