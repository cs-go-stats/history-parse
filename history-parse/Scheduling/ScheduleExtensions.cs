using System;
using System.Threading.Tasks;
using CSGOStats.Services.Core.Scheduling;
using Microsoft.Extensions.Configuration;
using Quartz;

namespace CSGOStats.Services.HistoryParse.Scheduling
{
    public static class ScheduleExtensions
    {
        public static async Task ConfigureJobsAsync(
            IScheduler scheduler,
            IServiceProvider serviceProvider,
            IConfigurationRoot configuration)
        {
            await scheduler.ScheduleJob(
                jobDetail: serviceProvider.CreateJobTemplate<DefaultJob>(),
                trigger: SchedulerExtensions.CreateCronScheduledTriggerFromConfiguration(configuration, "Jobs:Default:CronExpression"));
        }
    }
}