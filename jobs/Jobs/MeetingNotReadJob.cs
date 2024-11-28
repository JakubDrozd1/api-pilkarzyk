using BLLLibrary.IService;
using Microsoft.Extensions.Logging;
using Quartz;
using DataLibrary.Model.DTO.Response;
using DataLibrary.Entities;
using DataLibrary.Helper.Notification;
using DataLibrary.Model.DTO.Request.TableRequest;


namespace Jobs.Jobs
{
    [DisallowConcurrentExecution]
    public class MeetingNotReadJob : IJob
    {
        private readonly ILogger<MeetingNotReadJob> _logger;
        private readonly IMeetingsService _meetingsService;
        private readonly INotificationService _notificationService;
        private readonly INotificationTokenService _notificationTokenService;


        public MeetingNotReadJob(
            ILogger<MeetingNotReadJob> logger,
            IMeetingsService meetingsService,
            INotificationService notificationService,
            INotificationTokenService notificationTokenService
            )
        {
            _logger = logger;
            _meetingsService = meetingsService;
            _notificationService = notificationService;
            _notificationTokenService = notificationTokenService;
        }

        public async Task Execute(IJobExecutionContext context)
        {

            var meetings = await _meetingsService.GetAllActualMeetingsAsync();
            var meetingsDictionary = new Dictionary<int, GetMeetingReminderResponse>();
            var meetingsToFilterDictionary = new Dictionary<int, int>();

            FirebaseNotification notificationHub = new();

            foreach (var meetingUser in meetings)
            {
                if (meetingUser != null && meetingUser.IdMeeting != null && meetingUser.Answer == "yes")
                {
                    if (!meetingsToFilterDictionary.ContainsKey((int)meetingUser.IdMeeting))
                    {
                        meetingsToFilterDictionary[(int)meetingUser.IdMeeting] = 0;

                    }
                    meetingsToFilterDictionary[(int)meetingUser.IdMeeting] += 1;
                }
            }
            try
            {
                foreach (var meetingUser in meetings)
                {
                    if (
                        meetingUser != null &&
                        meetingUser.Answer != "yes" &&
                        meetingUser.IdMeeting != null &&
                        (!meetingsToFilterDictionary.ContainsKey((int)meetingUser.IdMeeting) || meetingsToFilterDictionary[(int)meetingUser.IdMeeting] < meetingUser.Quantity) &&
                        meetingUser.LastReminderMessagesTime != null &&
                        meetingUser.ReminderMessagesTime != null &&
                        ((DateTime)meetingUser.LastReminderMessagesTime).AddMinutes((int)meetingUser.ReminderMessagesTime) <= DateTime.Now &&
                        meetingUser.WaitingTimeDecision != null &&
                        meetingUser.DateMeeting != null &&
                        ((DateTime)meetingUser.DateMeeting).AddMinutes(-(int)meetingUser.WaitingTimeDecision) >= DateTime.Now
                        )
                    {
                        NOTIFICATION? userDetails = await _notificationService.GetAllNotificationFromUser(meetingUser.IdUser ?? 0);
                        if (userDetails != null)
                        {
                            var actualTime = (DateTime.Now).TimeOfDay;
                            if (userDetails.MEETING_REMINDER_NOTIFICATION && (
                                    (
                                        userDetails.TIME_SILENT_START > userDetails.TIME_SILENT_END &&
                                        (userDetails.TIME_SILENT_START >= actualTime &&
                                        userDetails.TIME_SILENT_END <= actualTime)
                                    ) ||
                                    (
                                        userDetails.TIME_SILENT_START < userDetails.TIME_SILENT_END &&
                                        (userDetails.TIME_SILENT_START >= actualTime ||
                                        userDetails.TIME_SILENT_END <= actualTime)
                                    ) ||
                                    userDetails.TIME_SILENT_START == userDetails.TIME_SILENT_END
                                ))
                            {
                                var tokens = await _notificationTokenService.GetAllTokensFromUser(meetingUser.IdUser ?? throw new Exception("User is null"));
                                var title = "Nie zapomnij o spotkaniu w " + meetingUser.Place;
                                var body = meetingUser.DateMeeting?.ToString("dd-MM-yyyy HH:mm") + " " + meetingUser.Place + " " + meetingUser.Description;
                                if (tokens != null)
                                {
                                    await notificationHub.SendMeetingNotification(tokens, title, body, meetingUser.IdMeeting ?? 0);
                                }
                                await _notificationService.AddNotificationMessageToUserAsync(
                                    new GetNotificationMessageRequest
                                    {
                                        IDUSER = meetingUser.IdUser ?? 0,
                                        IDGROUP = meetingUser.IdGroup,
                                        IDMEETING = meetingUser.IdMeeting,
                                        DATE_SEND = DateTime.Now,
                                        TITLE = title,
                                        BODY = body,
                                        SENDED = true,
                                        REPEAT = true,
                                        NOTIFICATION_TYPE = 1
                                    }
                                );
                            }
                        }
                        meetingsDictionary[meetingUser.IdMeeting ?? throw new Exception()] = meetingUser;
                    }
                }
            }
            catch (Exception)
            {
            }

            foreach (var meeting in meetingsDictionary)
            {
                var meetingToUpdate = await _meetingsService.GetMeetingByIdAsync(meeting.Key);

                if (meetingToUpdate != null)
                {
                    MEETINGS meetingToJob = new()
                    {
                        ID_MEETING = meeting.Key,
                        DESCRIPTION = meeting.Value.Description,
                        QUANTITY = meeting.Value.Quantity,
                        DATE_MEETING = meeting.Value.DateMeeting,
                        PLACE = meeting.Value.Place,
                        IDGROUP = meeting.Value.IdGroup,
                        IDAUTHOR = meeting.Value.IdAuthor,
                        IS_INDEPENDENT = meeting.Value.IsIndependent,
                        LAST_REMINDER_MESSAGES_TIME = DateTime.Now,
                        WAITING_TIME_DECISION = meeting.Value.WaitingTimeDecision,
                        DATE_QUEST_END = meetingToUpdate.DateQuestEnd,
                        DATE_QUEST_OPEN = meetingToUpdate.DateQuestOpen,
                        IS_QUEST = meetingToUpdate.IsQuest,
                        MAX_GIVE_ME_TIME = meetingToUpdate.MaxGiveMeTime,
                        REMINDER_MESSAGES_TIME = meetingToUpdate.ReminderMessagesTime,
                        CANCELED = meetingToUpdate.Canceled
                    };

                    await _meetingsService.UpdateMeetingJobAsync(meetingToJob);
                }
            }
        }
    }
}
