using System;
using System.Threading.Tasks;
using CSGOStats.Infrastructure.Extensions;
using CSGOStats.Services.HistoryParse.Processing;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace CSGOStats.Services.HistoryParse.Scheduling
{
    public class DefaultJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            var serviceProvider = context.MergedJobDataMap.Get("ServiceProvider").OfType<IServiceProvider>();

            using var scope = serviceProvider.CreateScope();
            await scope.ServiceProvider.GetService<Processor>().RunAsync();
        }
    }
}