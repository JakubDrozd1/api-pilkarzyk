using System.Data;
using Dapper;
using DataLibrary.Entities;
using DataLibrary.Helper;
using DataLibrary.Helper.Notification;
using DataLibrary.IRepository.UsersMeetings;
using DataLibrary.Model.DTO.Request;
using FirebirdSql.Data.FirebirdClient;
using Microsoft.AspNetCore.SignalR;

namespace DataLibrary.Repository.UsersMeetings
{
    public class CreateUsersMeetingsRepository(FbConnection dbConnection, IHubContext<NotificationHub, INotificationHub> hubContext, FbTransaction? fbTransaction) : ICreateUsersMeetingsRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;
        private readonly IHubContext<NotificationHub, INotificationHub> _productNotification = hubContext;
        private readonly FbTransaction? _fbTransaction = fbTransaction;

        public async Task AddUserToMeetingAsync(MEETINGS meeting, USERS user)
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                await _dbConnection.OpenAsync();
            }
            try
            {
                USERS_MEETINGS usersMeetings = new()
                {
                    IDMEETING = meeting.ID_MEETING ?? throw new Exception("Meeting is null"),
                    IDUSER = user.ID_USER,
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

        public async Task SendNotification(GetUsersMeetingsRequest getUsersMeetingsRequest)
        {
            try
            {
                foreach (int userId in getUsersMeetingsRequest.IdUsers)
                {
                    await _productNotification.Clients.All.SendMessage(userId, getUsersMeetingsRequest.IdMeeting);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
