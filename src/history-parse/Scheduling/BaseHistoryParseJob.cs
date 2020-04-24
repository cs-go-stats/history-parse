using System;
using System.Threading.Tasks;
using CSGOStats.Infrastructure.Core.Scheduling;
using CSGOStats.Services.HistoryParse.Processing;
using Microsoft.Extensions.DependencyInjection;

namespace CSGOStats.Services.HistoryParse.Scheduling
{
    public abstract class BaseHistoryParseJob : BaseJob
    {
        protected abstract bool IsForcedRun { get; }

        protected override Task ExecuteAsync(IServiceProvider serviceProvider) =>
            serviceProvider.GetService<Processor>().RunAsync(isForcedRun: IsForcedRun);
    }
}