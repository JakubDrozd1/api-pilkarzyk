using Dapper;
using System.Data;
using DataLibrary.Entities;
using DataLibrary.Helper;
using DataLibrary.IRepository.Notification;
using FirebirdSql.Data.FirebirdClient;
using DataLibrary.Model.DTO.Request.Pagination;
using DataLibrary.Model.DTO.Response;
using static Dapper.SqlMapper;

namespace DataLibrary.Repository.Notification
{
    internal class ReadNotificationRepository(FbConnection dbConnection, FbTransaction? fbTransaction) : IReadNotificationRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;
        private readonly FbTransaction? _fbTransaction = fbTransaction;

        private readonly string SELECT = $"COALESCE({nameof(NOTIFICATION.MEETING_NOTIFICATION)}, true) AS MEETING_NOTIFICATION, " +
            $"COALESCE({nameof(NOTIFICATION.GROUP_INV_NOTIFICATION)}, true) AS GROUP_INV_NOTIFICATION, " +
            $"COALESCE({nameof(NOTIFICATION.MEETING_ORGANIZER_NOTIFICATION)}, true) AS MEETING_ORGANIZER_NOTIFICATION, " +
            $"COALESCE({nameof(NOTIFICATION.TEAM_NOTIFICATION)}, true) AS TEAM_NOTIFICATION, " +
            $"COALESCE({nameof(NOTIFICATION.UPDATE_MEETING_NOTIFICATION)}, true) AS UPDATE_MEETING_NOTIFICATION, " +
            $"COALESCE({nameof(NOTIFICATION.TEAM_ORGANIZER_NOTIFICATION)}, true) AS TEAM_ORGANIZER_NOTIFICATION, " +
            $"COALESCE({nameof(NOTIFICATION.GROUP_ADD_NOTIFICATION)}, true) AS GROUP_ADD_NOTIFICATION, " +
            $"COALESCE({nameof(NOTIFICATION.MEETING_CANCEL_NOTIFICATION)}, true) AS MEETING_CANCEL_NOTIFICATION, " +
            $"COALESCE({nameof(NOTIFICATION.MEETING_REMINDER_NOTIFICATION)}, true) AS MEETING_REMINDER_NOTIFICATION, " +
            $"{nameof(NOTIFICATION.TIME_SILENT_START)} AS TIME_SILENT_START, " +
            $"{nameof(NOTIFICATION.TIME_SILENT_END)} AS TIME_SILENT_END, " +
            $"COALESCE({nameof(NOTIFICATION.IDUSER)}, 0) AS IDUSER, " +
            $"COALESCE({nameof(NOTIFICATION.ID_NOTIFICATION)}, 0) AS ID_NOTIFICATION ";


        public async Task<NOTIFICATION?> GetAllNotificationFromUser(int userId)
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                await _dbConnection.OpenAsync();
            }
            try
            {
                var query = new QueryBuilder<NOTIFICATION>()
                    .Select(SELECT)
                    .From(" NOTIFICATION n RIGHT JOIN USERS u ON u.ID_USER = n.IDUSER ")
                    .Where("ID_USER = @IdUser AND u.IS_ACTIVE = true ");

                return await _dbConnection.QuerySingleOrDefaultAsync<NOTIFICATION>(query.Build(), new { IdUser = userId }, _fbTransaction);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

        public async Task<List<GetNotificationMessageResponse>> GetAllNotificationMessageFromUser(GetNotificationMessagePaginationRequest getNotificationMessagePaginationRequest)
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                await _dbConnection.OpenAsync();
            }
            try
            {
                DynamicParameters dynamicParameters = new();
                string WHERE = "1=1 ";
                if (getNotificationMessagePaginationRequest.IdUser is not null)
                {
                    WHERE += $"AND {nameof(NOTIFICATION_MESSAGES.IDUSER)} = @UserId ";
                    dynamicParameters.Add("@UserId", getNotificationMessagePaginationRequest.IdUser);
                }
                if (getNotificationMessagePaginationRequest.Sended is not null)
                {
                    WHERE += $"AND {nameof(NOTIFICATION_MESSAGES.SENDED)} = @Sended ";
                    dynamicParameters.Add("@Sended", getNotificationMessagePaginationRequest.Sended);
                }
                if (getNotificationMessagePaginationRequest.Repeat is not null)
                {
                    WHERE += $"AND {nameof(NOTIFICATION_MESSAGES.REPEAT)} = @Repeat ";
                    dynamicParameters.Add("@Repeat", getNotificationMessagePaginationRequest.Repeat);
                }
                if (getNotificationMessagePaginationRequest.NotificationType is not null)
                {
                    WHERE += $"AND {nameof(NOTIFICATION_MESSAGES.NOTIFICATION_TYPE)} = @NotificationType ";
                    dynamicParameters.Add("@NotificationType", getNotificationMessagePaginationRequest.NotificationType);
                }
                string SELECT
                    = $"{nameof(NOTIFICATION_MESSAGES.DATE_SEND)} AS DateSend, " +
                $"{nameof(NOTIFICATION_MESSAGES.IDUSER)} AS IdUser, " +
                $"{nameof(NOTIFICATION_MESSAGES.IDGROUP)} AS IdGroup, " +
                $"{nameof(NOTIFICATION_MESSAGES.IDMEETING)} AS IdMeeting, " +
                $"{nameof(NOTIFICATION_MESSAGES.TITLE)}, " +
                $"{nameof(NOTIFICATION_MESSAGES.SENDED)}, " +
                $"{nameof(NOTIFICATION_MESSAGES.REPEAT)}, " +
                $"{nameof(NOTIFICATION_MESSAGES.NOTIFICATION_TYPE)} AS NotificationType, " +
                $"{nameof(NOTIFICATION_MESSAGES.ID_NOTIFICATION_MESSAGES)} AS IdNotificationMessage, " +
                $"{nameof(NOTIFICATION_MESSAGES.BODY)} ";

                var query = new QueryBuilder<List<GetNotificationMessageResponse>>()
                            .Select(SELECT)
                            .From($"{nameof(NOTIFICATION_MESSAGES)} ")
                            .Where(WHERE)
                            .OrderBy(getNotificationMessagePaginationRequest)
                            .Limit(getNotificationMessagePaginationRequest);

                return (await _dbConnection.QueryAsync<GetNotificationMessageResponse>(query.Build(), dynamicParameters, _fbTransaction)).AsList();
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
