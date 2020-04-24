using System;
using System.Threading.Tasks;
using CSGOStats.Infrastructure.Core.Scheduling.Extensions;
using Microsoft.Extensions.Configuration;
using Quartz;

namespace CSGOStats.Services.HistoryParse.Scheduling
{
    public static class ScheduleExtensions
    {
        public static Task ConfigureJobsAsync(
            IScheduler scheduler,
            IServiceProvider serviceProvider,
            IConfigurationRoot configuration)
        {
            return Task.WhenAll(
                scheduler.ScheduleJobAsync<DefaultJob>(serviceProvider, configuration),
                scheduler.ScheduleJobAsync<ForcedJob>(serviceProvider, configuration));
        }
    }
}