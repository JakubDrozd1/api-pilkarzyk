using BLLLibrary.IService;
using Dapper;
using DataLibrary.Entities;
using DataLibrary.Helper;
using DataLibrary.Model.DTO.Request.TableRequest;
using DataLibrary.Model.DTO.Request;
using Microsoft.Extensions.Logging;
using Quartz;


namespace Jobs.Jobs
{
    [DisallowConcurrentExecution]
    public class DateQuestJob : IJob
    {
        private readonly ILogger<DateQuestJob> _logger;
        private readonly IMeetingsService _meetingsService;
        private readonly IDateQuestsService _dateQuestsService;

        public DateQuestJob(ILogger<DateQuestJob> logger, IMeetingsService meetingsService, IDateQuestsService dateQuestsService)
        {
            _logger = logger;
            _meetingsService = meetingsService;
            _dateQuestsService = dateQuestsService;

        }
        public Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("Simple schedule background job");
            _logger.LogInformation("Job has been working");

            var mettings = _meetingsService.GetAllMeetingsWithQuest();

            mettings.ForEach((metting) =>
            {
                var idMetting = metting.IdMeeting;

                /*if (idMetting != null)
                {
                    var dateQuests =  _dateQuestsService.GetDateQuestByMeetingId((int)idMetting);
                    dateQuests
                    .OrderBy((dateQuest) => dateQuest.USERS_DATE_QUESTS.Count)
                    .OrderBy((dateQuest) => dateQuest.DATE_MEETING);
                    var selectedDate = dateQuests.FirstOrDefault();

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
                        _meetingsService.UpdateMeeting(meetingToUpdate);
                    }
                }*/
            });


            return Task.CompletedTask;
        }
    }
}
