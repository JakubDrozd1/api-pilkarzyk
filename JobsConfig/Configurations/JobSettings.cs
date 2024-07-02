namespace JobsConfig.Configurations
{
    public class JobSettings
    {
        public List<JobDetail> Jobs { get; set; }
        public JobSettings()
        {
            Jobs = new List<JobDetail>();
        }

    }
    public class JobDetail
    {
        public required string TypeName { get; set; }

        public required string JobKey { get; set; }
        public string? CronSchedule { get; set; }
        public int? IntervalMiliSeconds { get; set; }
        public bool IsActive { get; set; }
    }
}
