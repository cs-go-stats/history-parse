using System.Threading.Tasks;
using CSGOStats.Infrastructure.DataAccess;
using CSGOStats.Infrastructure.PageParse.Page.Parsing;
using CSGOStats.Services.Core.Initialization;
using CSGOStats.Services.HistoryParse.Aggregate;
using CSGOStats.Services.HistoryParse.Aggregate.Data;
using CSGOStats.Services.HistoryParse.Config;
using CSGOStats.Services.HistoryParse.Data;
using CSGOStats.Services.HistoryParse.Processing;
using CSGOStats.Services.HistoryParse.Processing.Page.Model.State;
using CSGOStats.Services.HistoryParse.Processing.Parsing;
using CSGOStats.Services.HistoryParse.Scheduling;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CSGOStats.Services.HistoryParse
{
    internal static class Program
    {
        private static async Task Main()
        {
#if DEBUG
            var environment = Environments.Development;
#else
            var environment = Environments.Production;
#endif
            var startupBuilder = await Startup
                .ForEnvironment(Service.Name, environment)
                .WithMessaging<Service>()
                .UsesPostgres()
                .WithJobsAsync(ScheduleExtensions.ConfigureJobsAsync);
            await startupBuilder
                .ConfigureServices(ConfigureServiceProvider)
                .RunAsync(
                    actionBeforeStart: services => services.EnsureDatabaseCreated());
        }

        private static IServiceCollection ConfigureServiceProvider(this IServiceCollection services, IConfigurationRoot configuration)
        {
            return services
                .AddScoped<Processor>()
                .AddScoped<IPageParser<HistoryPageModel>, Parser>()
                .ConfigureMatchSettings(configuration)
                .ConfigureDatabase(configuration)
                .AddScoped<AggregateFacade>();
        }

        private static IServiceCollection ConfigureDatabase(this IServiceCollection services, IConfigurationRoot configuration) =>
            services
                .AddDataAccessConfiguration(configuration, usesPostgres: true)
                .RegisterPostgresContext<HistoryParseContext>()
                .RegisterPostgresRepositoryFor<Aggregate.Entities.HistoryParse>();
    }
}
