using Microsoft.Extensions.Logging;
using Quartz;


namespace QuartzInfrastructure.Jobs
{
    [DisallowConcurrentExecution]
    public class DateQuestJob : IJob
    {
        private readonly ILogger<DateQuestJob> _logger;
        public DateQuestJob(ILogger<DateQuestJob> logger)
        {
            _logger = logger;

        }
        public Task Execute(IJobExecutionContext context)
        {
            return Task.CompletedTask;
        }
    }
}
