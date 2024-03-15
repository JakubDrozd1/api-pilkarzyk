using System.Data;
using Dapper;
using DataLibrary.Entities;
using DataLibrary.Helper;
using DataLibrary.IRepository.UsersMeetings;
using DataLibrary.Model.DTO.Request.TableRequest;
using DataLibrary.Model.DTO.Response;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository.UsersMeetings
{
    public class CreateUsersMeetingsRepository(FbConnection dbConnection, FbTransaction? fbTransaction) : ICreateUsersMeetingsRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;
        private readonly FbTransaction? _fbTransaction = fbTransaction;

        public async Task AddUserToMeetingAsync(GetMeetingGroupsResponse meeting, int userId)
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                await _dbConnection.OpenAsync();
            }
            try
            {
                GetUserMeetingRequest usersMeetings = new()
                {
                    IDMEETING = meeting.IdMeeting ?? throw new Exception("Meeting is null"),
                    IDUSER = userId,
                };
                var insertBuilder = new QueryBuilder<USERS_MEETINGS>().Insert("USERS_MEETINGS ", usersMeetings);
                string insertQuery = insertBuilder.Build();
                await _dbConnection.ExecuteAsync(insertQuery, usersMeetings, _fbTransaction);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
