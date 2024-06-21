using BLLLibrary.IService;
using Dapper;
using DataLibrary.Entities;
using DataLibrary.Helper;
using DataLibrary.Model.DTO.Request.TableRequest;
using DataLibrary.Model.DTO.Request;
using Microsoft.Extensions.Logging;
using Quartz;
using BLLLibrary.Service;


namespace Jobs.Jobs
{
    [DisallowConcurrentExecution]
    public class DateQuestJob : IJob
    {
        private readonly ILogger<DateQuestJob> _logger;
        private readonly IMeetingsService _meetingsService;
        private readonly IDateQuestsService _dateQuestsService;
        private readonly IMessagesService _messageService;

        public DateQuestJob(ILogger<DateQuestJob> logger, IMeetingsService meetingsService, IDateQuestsService dateQuestsService, IMessagesService messagesService)
        {
            _logger = logger;
            _meetingsService = meetingsService;
            _dateQuestsService = dateQuestsService;
            _messageService = messagesService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("Simple schedule background job");
            _logger.LogInformation("Job has been working");

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
                         _logger.LogInformation($"SelectedDate: {selectedDate.USERS_DATE_QUESTS.Count} {selectedDate.DATE_MEETING}");

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
                     }
                }
            }


        }
    }
}
