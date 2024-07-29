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
        public async Task<List<GetGuestsMeetingsResponse?>> GetAllGuestFromMeetingAsync(int meetingId)
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                await _dbConnection.OpenAsync();
            }
            try
            {
                var query = new QueryBuilder<GetGuestsMeetingsResponse>()
                    .Select($"gue.*, t.{nameof(TEAMS.ID_TEAM)}, t.{nameof(TEAMS.COLOR)} AS TeamColor, m.{nameof(MEETINGS.CANCELED)} AS Canceled ")
                    .From(
                        $"{nameof(GUESTS)} gue " +
                        $"LEFT JOIN {nameof(TEAMS)} t ON gue.{nameof(GUESTS.IDTEAM)} = t.{nameof(TEAMS.ID_TEAM)} " +
                        $"LEFT JOIN {nameof(MEETINGS)} m ON gue.{nameof(GUESTS.IDMEETING)} = m.{nameof(MEETINGS.ID_MEETING)} ")
                    .Where("gue.IDMEETING = @MeetingId ");

                return (await _dbConnection.QueryAsync<GetGuestsMeetingsResponse?>(query.Build(), new { MeetingId = meetingId }, _fbTransaction)).AsList();
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
