using BLLLibrary.IService;
using Dapper;
using DataLibrary.Entities;
using DataLibrary.Helper;
using Microsoft.Extensions.Logging;
using Quartz;


namespace Jobs.Jobs
{
    [DisallowConcurrentExecution]
    public class DateQuestJob : IJob
    {
        private readonly ILogger<DateQuestJob> _logger;
        private readonly IMeetingsService _meetingsService;

        public DateQuestJob(ILogger<DateQuestJob> logger, IMeetingsService meetingsService)
        {
            _logger = logger;
            _meetingsService = meetingsService;

        }
        public Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("Simple schedule background job");
            _logger.LogInformation("Job has been working");

            var metting = _meetingsService.GetAllMeetingsWithQuest();

            _logger.LogInformation($"Metting: {metting.Count}");

            return Task.CompletedTask;
        }
    }
}
