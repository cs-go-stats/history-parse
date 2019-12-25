using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CSGOStats.Extensions.Validation;
using CSGOStats.Services.HistoryParse.Config;
using CSGOStats.Services.HistoryParse.Config.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace CSGOStats.Services.HistoryParse.Scheduling
{
    public class ScheduleBuilder
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfigurationRoot _configuration;

        public ScheduleBuilder(IServiceProvider serviceProvider, IConfigurationRoot configuration)
        {
            _serviceProvider = serviceProvider.NotNull(nameof(serviceProvider));
            _configuration = configuration.NotNull(nameof(configuration));
        }

        public async Task<IScheduler> CreateAsync()
        {
            var factory = _serviceProvider.GetService<ISchedulerFactory>();
            var scheduler = await factory.GetScheduler();

            await scheduler.ScheduleJob(
                jobDetail: JobBuilder.Create<DefaultJob>()
                    .SetJobData(
                        new JobDataMap(
                            (IDictionary<string, object>)new Dictionary<string, object>
                            {
                                ["ServiceProvider"] = _serviceProvider
                            }))
                    .StoreDurably()
                    .Build(),
                trigger: TriggerBuilder
                    .Create()
                    .WithSchedule(
                        CronScheduleBuilder.CronSchedule(_configuration
                            .GetFromConfiguration(
                                sectionName: "Cycle",
                                creatingFunctor: configurationSection => new CycleSetting(
                                    cronExpression: configurationSection["CronExpression"]))
                            .CronExpression))
                    .Build());

            return scheduler;
        }
    }
}