using BLLLibrary.IService;
using Microsoft.Extensions.Logging;
using Quartz;
using DataLibrary.UoW;
using DataLibrary.Model.DTO.Request.Pagination;
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
        private readonly IMessagesService _messageService;
        IUnitOfWork _unitOfWork;

        public MeetingNotReadJob(
            ILogger<MeetingNotReadJob> logger,
            IMeetingsService meetingsService,
            IMessagesService messagesService,
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _meetingsService = meetingsService;
            _messageService = messagesService;
            _unitOfWork = unitOfWork;
        }

        public async Task Execute(IJobExecutionContext context)
        {

            var mettings = await _meetingsService.GetAllActualMeetingsAsync();
            var meetingsDictionary = new Dictionary<int, GetMeetingReminderResponse>();


            FirebaseNotification notificationHub = new();

            foreach (var mettingUser in mettings)
            {
                if (mettingUser.IdMeeting != null &&
                    mettingUser.LastReminderMessagesTime != null &&
                    mettingUser.ReminderMessagesTime != null &&
                    ((DateTime)mettingUser.LastReminderMessagesTime).AddMinutes((int)mettingUser.ReminderMessagesTime) <= DateTime.Now)
                {
                    NOTIFICATION? userDetails = await _unitOfWork.ReadNotificationRepository.GetAllNotificationFromUser(mettingUser.IdUser ?? 0);
                    if (userDetails != null)
                    {
                        if (userDetails.MEETING_NOTIFICATION)
                        {
                            var tokens = await _unitOfWork.ReadNotificationTokenRepository.GetAllTokensFromUser(mettingUser.IdUser ?? throw new Exception("User is null"));

                            if (tokens != null)
                            {
                                await notificationHub.SendMeetingReminderNotification(mettingUser, tokens);
                            }
                        }
                    }
                    meetingsDictionary[(int)mettingUser.IdMeeting] = mettingUser;
                }
            }

            foreach (var meeting in meetingsDictionary)
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
                };

                _meetingsService.UpdateMeetingJobAsync(meetingToJob);

            }
        }
    }
}
