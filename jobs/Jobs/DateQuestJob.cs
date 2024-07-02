using BLLLibrary.IService;
using Dapper;
using DataLibrary.Entities;
using DataLibrary.Helper;
using DataLibrary.Model.DTO.Request.TableRequest;
using DataLibrary.Model.DTO.Request;
using Microsoft.Extensions.Logging;
using Quartz;
using BLLLibrary.Service;
using DataLibrary.Helper.Notification;
using DataLibrary.UoW;
using DataLibrary.Model.DTO.Request.Pagination;


namespace Jobs.Jobs
{
    [DisallowConcurrentExecution]
    public class DateQuestJob : IJob
    {
        private readonly ILogger<DateQuestJob> _logger;
        private readonly IMeetingsService _meetingsService;
        private readonly IDateQuestsService _dateQuestsService;
        private readonly IMessagesService _messageService;
        IUnitOfWork _unitOfWork;

        public DateQuestJob(
            ILogger<DateQuestJob> logger,
            IMeetingsService meetingsService,
            IDateQuestsService dateQuestsService,
            IMessagesService messagesService,
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _meetingsService = meetingsService;
            _dateQuestsService = dateQuestsService;
            _messageService = messagesService;
            _unitOfWork = unitOfWork;
        }

        public async Task Execute(IJobExecutionContext context)
        {

            var mettings = await _meetingsService.GetAllMeetingsWithQuestAsync();

            foreach(var metting in mettings) 
            {
                var idMetting = metting.IdMeeting;

                if (idMetting != null)
                {
                    var dateQuests =  await _dateQuestsService.GetDateQuestByMeetingIdAsync((int)idMetting);
                    
                    var selectedDate = dateQuests
                        .OrderByDescending((dateQuest) => dateQuest.USERS_DATE_QUESTS.Count)
                        .ThenBy((dateQuest) => dateQuest.DATE_MEETING)
                        .FirstOrDefault();

                     if (selectedDate != null)
                     {

                         MEETINGS meetingToUpdate = new()
                         {
                             ID_MEETING = idMetting,
                             DATE_QUEST_END = null,
                             DATE_QUEST_OPEN = false,
                             IS_QUEST = false,
                             DATE_MEETING = selectedDate.DATE_MEETING,
                             DESCRIPTION = metting.Description,
                             IDAUTHOR = metting.IdAuthor,
                             IDGROUP = metting.IdGroup,
                             IS_INDEPENDENT = metting.IsIndependent,
                             PLACE = metting.Place,
                             QUANTITY = metting.Quantity,
                             WAITING_TIME_DECISION = metting.WaitingTimeDecision,
                         };
                         await _meetingsService.UpdateMeetingJobAsync(meetingToUpdate);

                         foreach (var userDateQuest in selectedDate.USERS_DATE_QUESTS)
                         {
                            var messageToChange = new GetMessageRequest {
                                ANSWER = "yes",
                                IDMEETING = idMetting,
                                IDUSER = userDateQuest.IDUSER,
                            };

                            await _messageService.UpdateAnswerMessageAsync(messageToChange);
                         }

                        FirebaseNotification notificationHub = new();
                        var meeting = await _unitOfWork.ReadMeetingsRepository.GetMeetingByIdAsync((int)idMetting) ?? throw new Exception("Meeting is null");
                        var users = await _unitOfWork.ReadGroupsUsersRepository.GetListGroupsUserAsync(new GetUsersGroupsPaginationRequest()
                        {
                            Page = 0,
                            OnPage = -1,
                            IdGroup = metting.IdGroup,
                            IsAvatar = false
                        });
                        foreach (var user in users)
                        {
                            NOTIFICATION? userDetails = await _unitOfWork.ReadNotificationRepository.GetAllNotificationFromUser(user.IdUser ?? 0);
                            if (userDetails != null)
                            {
                                if (userDetails.MEETING_NOTIFICATION)
                                {
                                        var tokens = await _unitOfWork.ReadNotificationTokenRepository.GetAllTokensFromUser(user.IdUser ?? throw new Exception("User is null"));

                                        if (tokens != null)
                                        {
                                            await notificationHub.SendMeetingQuestEndNotification(meeting, tokens);
                                        }
                                }
                            }
                        }
                    }
                }
            }


        }
    }
}
