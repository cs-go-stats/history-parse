using System.Threading.Tasks;
using CSGOStats.Infrastructure.Validation;
using CSGOStats.Services.HistoryParse.Scheduling;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CSGOStats.Services.HistoryParse
{
    public class Startup 
    {
        private readonly IConfigurationRoot _configuration;
        private readonly ServiceProvider _serviceProvider;

        public Startup(IConfigurationRoot configuration, ServiceProvider serviceProvider)
        {
            _configuration = configuration.NotNull(nameof(configuration));
            _serviceProvider = serviceProvider.NotNull(nameof(serviceProvider));
        }

        public async Task<Runtime> CreateRuntimeAsync() => new Runtime(
            serviceProvider:_serviceProvider,
            scheduler: await new ScheduleBuilder(_serviceProvider, _configuration).CreateAsync());
    }
}