using System;
using System.Threading.Tasks;
using CSGOStats.Services.Core.Scheduling;
using CSGOStats.Services.HistoryParse.Processing;
using Microsoft.Extensions.DependencyInjection;

namespace CSGOStats.Services.HistoryParse.Scheduling
{
    public class DefaultJob : JobBase
    {
        protected override string Code => nameof(DefaultJob);

        protected override Task ExecuteAsync(IServiceProvider serviceProvider)=>
            serviceProvider.GetService<Processor>().RunAsync();
    }
}