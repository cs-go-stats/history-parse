using System.Threading.Tasks;
using CSGOStats.Infrastructure.DataAccess.Contexts;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace CSGOStats.Services.HistoryParse
{
    public class Runtime
    {
        private readonly ServiceProvider _serviceProvider;
        private readonly IScheduler _scheduler;

        public Runtime(ServiceProvider serviceProvider, IScheduler scheduler)
        {
            _serviceProvider = serviceProvider;
            _scheduler = scheduler;
        }

        public async Task RunAsync()
        {
            await _serviceProvider.GetService<BaseDataContext>().Database.EnsureCreatedAsync();
            await _scheduler.Start();
        }

        public async Task ShutdownAsync()
        {
            await _scheduler.Shutdown(false);
            await _serviceProvider.DisposeAsync();
        }
    }
}