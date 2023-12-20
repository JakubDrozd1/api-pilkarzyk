using System.Data;
using Dapper;
using DataLibrary.Entities;
using DataLibrary.Helper;
using DataLibrary.Helper.Notification;
using DataLibrary.IRepository.UsersMeetings;
using DataLibrary.Model.DTO.Request;
using DataLibrary.Repository.Meetings;
using DataLibrary.Repository.Messages;
using DataLibrary.Repository.Users;
using FirebirdSql.Data.FirebirdClient;
using Microsoft.AspNetCore.SignalR;

namespace DataLibrary.Repository.UsersMeetings
{
    public class CreateUsersMeetingsRepository(FbConnection dbConnection, IHubContext<NotificationHub, INotificationHub> hubContext) : ICreateUsersMeetingsRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;
        private readonly IHubContext<NotificationHub, INotificationHub> _productNotification = hubContext;

        public async Task AddUserToMeetingAsync(int idMeeting, int idUser, FbTransaction? transaction = null)
        {
            FbConnection db = transaction?.Connection ?? _dbConnection;
            if (transaction == null && db.State != ConnectionState.Open)
            {
                await db.OpenAsync();
            }
            var localTransaction = transaction ?? await db.BeginTransactionAsync();
            try
            {
                var readMeetingsRepository = new ReadMeetingsRepository(db);
                var readUsersRepository = new ReadUsersRepository(db);
                var readGroupsUsersRepository = new ReadUsersMeetingsRepository(db);
                var meeting = await readMeetingsRepository.GetMeetingByIdAsync(idMeeting, localTransaction) ?? throw new Exception("Meeting is null");
                var user = await readUsersRepository.GetUserByIdAsync(idUser, localTransaction) ?? throw new Exception("User is null");

                int meetingId = meeting.ID_MEETING ?? throw new Exception("Meeting is null");
                if (await readGroupsUsersRepository.GetUserWithMeeting(meetingId, user.ID_USER, localTransaction) != null)
                {
                    throw new Exception("User is already in this meeting");
                }
                USERS_MEETINGS usersMeetings = new()
                {
                    IDMEETING = meetingId,
                    IDUSER = user.ID_USER,
                };
                var insertBuilder = new QueryBuilder<USERS_MEETINGS>().Insert("USERS_MEETINGS ", usersMeetings);
                string insertQuery = insertBuilder.Build();
                await db.ExecuteAsync(insertQuery, usersMeetings, localTransaction);
                if (transaction == null)
                {
                    await localTransaction.CommitAsync();
                }
            }
            catch (Exception ex)
            {
                if (transaction == null)
                    localTransaction?.RollbackAsync();
                throw new Exception($"{ex.Message}");
            }
        }

        public async Task AddUsersToMeetingAsync(GetUsersMeetingsRequest getUsersMeetingsRequest, FbTransaction? transaction = null)
        {
            FbConnection db = transaction?.Connection ?? _dbConnection;
            if (transaction == null && db.State != ConnectionState.Open)
            {
                await db.OpenAsync();
            }
            var localTransaction = transaction ?? await db.BeginTransactionAsync();
            try
            {
                CreateMessagesRepository createMessageRepository = new(db);
                foreach (int userId in getUsersMeetingsRequest.IdUsers)
                {
                    await AddUserToMeetingAsync(getUsersMeetingsRequest.IdMeeting, userId, localTransaction);
                    await createMessageRepository.AddMessageAsync(new MESSAGES()
                    {
                        IDUSER = userId,
                        IDMEETING = getUsersMeetingsRequest.IdMeeting
                    }, localTransaction);
                }
                if (transaction == null)
                {
                    await localTransaction.CommitAsync();
                }
                foreach (int userId in getUsersMeetingsRequest.IdUsers)
                {
                    await _productNotification.Clients.All.SendMessage(userId, getUsersMeetingsRequest.IdMeeting);
                }
            }
            catch (Exception ex)
            {
                localTransaction?.RollbackAsync();
                throw new Exception($"{ex.Message}");
            }

        }
    }
}
