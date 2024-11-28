using BLLLibrary.IService;
using DataLibrary.Helper.Notification;
using DataLibrary.Model.DTO.Request.Pagination;
using DataLibrary.Model.DTO.Request.TableRequest;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Jobs.Jobs
{
    internal class NotificationSendJob : IJob
    {
        private readonly ILogger<NotificationSendJob> _logger;
        private readonly INotificationService _notificationService;
        private readonly INotificationTokenService _notificationTokenService;


        public NotificationSendJob(
            ILogger<NotificationSendJob> logger,
            INotificationService notificationService,
            INotificationTokenService notificationTokenService)
        {
            _logger = logger;
            _notificationService = notificationService;
            _notificationTokenService = notificationTokenService;

        }
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                var notifications = await _notificationService.GetAllNotificationMessageFromUser(

                    new GetNotificationMessagePaginationRequest()
                    {
                        OnPage = -1,
                        Page = 0,
                        Sended = false,
                        Repeat = false,
                    }
                );
                FirebaseNotification notificationHub = new();
                if (notifications != null)
                {
                    foreach (var notification in notifications)
                    {
                        var tokens = await _notificationTokenService.GetAllTokensFromUser(notification.IdUser);

                        if (tokens != null)
                        {
                            switch (notification.NotificationType)
                            {
                                case 1:
                                    await notificationHub.SendMeetingNotification(tokens, notification.Title ?? "", notification.Body ?? "", notification.IdMeeting ?? 0);
                                    break;
                                case 2:
                                    await notificationHub.SendNotification(tokens, notification.Title ?? "", notification.Body ?? "");
                                    break;
                                case 3:
                                    await notificationHub.SendTeamNotification(tokens, notification.Title ?? "", notification.Body ?? "", notification.IdMeeting ?? 0);
                                    break;
                                case 4:
                                    await notificationHub.SendGroupNotification(tokens, notification.Title ?? "", notification.Body ?? "", notification.IdGroup ?? 0);
                                    break;
                            }
                        }
                        await _notificationService.UpdateColumnNotificationMessageAsync(new GetUpdateNotificationMessageRequest() { Column = ["SENDED"], SENDED = true }, notification.IdNotificationMessage ?? 0);
                    }
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
