using Dapper;
using System.Data;
using DataLibrary.Entities;
using DataLibrary.Helper;
using DataLibrary.IRepository.Notification;
using FirebirdSql.Data.FirebirdClient;

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
                    .Where("ID_USER = @IdUser ");
                return await _dbConnection.QuerySingleOrDefaultAsync<NOTIFICATION>(query.Build(), new { IdUser = userId }, _fbTransaction);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
