using Dapper;
using DataLibrary.Helper;
using System.Data;
using DataLibrary.IRepository.Notification;
using DataLibrary.Model.DTO.Request.TableRequest;
using FirebirdSql.Data.FirebirdClient;
using DataLibrary.Entities;

namespace DataLibrary.Repository.Notification
{
    internal class UpdateNotificationRepository(FbConnection dbConnection, FbTransaction? fbTransaction) : IUpdateNotificationRepository
    {

        private readonly FbConnection _dbConnection = dbConnection;
        private readonly FbTransaction? _fbTransaction = fbTransaction;
        public async Task UpdateColumnNotificationAsync(GetUpdateNotificationRequest getUpdateNotificationRequest, int userId)
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                await _dbConnection.OpenAsync();
            }
            try
            {
                DynamicParameters dynamicParameters = new();
                var updateBuilder = new QueryBuilder<GetUpdateNotificationRequest>()
                    .UpdateColumns($"{nameof(USERS)}", getUpdateNotificationRequest.Column)
                    .Where("IDUSER = @UserId");
                string updateQuery = updateBuilder.Build();
                dynamicParameters.Add("@UserId", userId);

                foreach (string column in getUpdateNotificationRequest.Column)
                {
                    switch (column)
                    {
                        case "MEETING_NOTIFICATION":
                            {
                                bool meetingNotification = getUpdateNotificationRequest.MEETING_NOTIFICATION;
                                dynamicParameters.Add($"@{column}", meetingNotification);
                            }
                            break;
                        case "GROUP_INV_NOTIFICATION":
                            {
                                bool grouopInvNotification = getUpdateNotificationRequest.GROUP_INV_NOTIFICATION;
                                dynamicParameters.Add($"@{column}", grouopInvNotification);
                            }
                            break;
                        case "MEETING_ORGANIZER_NOTIFICATION":
                            {
                                bool meetignOrganizerNotification = getUpdateNotificationRequest.MEETING_ORGANIZER_NOTIFICATION;
                                dynamicParameters.Add($"@{column}", meetignOrganizerNotification);
                            }
                            break;
                        case "TEAM_NOTIFICATION":
                            {
                                bool teamNotification = getUpdateNotificationRequest.TEAM_NOTIFICATION;
                                dynamicParameters.Add($"@{column}", teamNotification);
                            }
                            break;
                        case "UPDATE_MEETING_NOTIFICATION":
                            {
                                bool updateMeetingNotification = getUpdateNotificationRequest.UPDATE_MEETING_NOTIFICATION;
                                dynamicParameters.Add($"@{column}", updateMeetingNotification);
                            }
                            break;
                        case "TEAM_ORGANIZER_NOTIFICATION":
                            {
                                bool teamOrganizerNotification = getUpdateNotificationRequest.TEAM_ORGANIZER_NOTIFICATION;
                                dynamicParameters.Add($"@{column}", teamOrganizerNotification);
                            }
                            break;
                        case "GROUP_ADD_NOTIFICATION":
                            {
                                bool groupAddNotification = getUpdateNotificationRequest.GROUP_ADD_NOTIFICATION;
                                dynamicParameters.Add($"@{column}", groupAddNotification);
                            }
                            break;
                        case "MEETING_CANCEL_NOTIFICATION":
                            {
                                bool meetingCancelNotification = getUpdateNotificationRequest.MEETING_CANCEL_NOTIFICATION;
                                dynamicParameters.Add($"@{column}", meetingCancelNotification);
                            }
                            break;
                    }

                }
                await _dbConnection.ExecuteAsync(updateQuery, dynamicParameters, _fbTransaction);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
