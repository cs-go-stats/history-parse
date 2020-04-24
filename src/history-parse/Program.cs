using System;
using System.Threading.Tasks;
using CSGOStats.Infrastructure.Core.Data.Entities;
using CSGOStats.Infrastructure.Core.Data.Storage;
using CSGOStats.Infrastructure.Core.Extensions;
using CSGOStats.Infrastructure.Core.Initialization;
using CSGOStats.Infrastructure.Core.PageParse.Page.Parse;
using CSGOStats.Infrastructure.Core.Throttling;
using CSGOStats.Services.HistoryParse.Aggregate.Data;
using CSGOStats.Services.HistoryParse.Aggregate.Entities;
using CSGOStats.Services.HistoryParse.Aggregate.Factories;
using CSGOStats.Services.HistoryParse.Config;
using CSGOStats.Services.HistoryParse.Processing;
using CSGOStats.Services.HistoryParse.Processing.Page.Model.State;
using CSGOStats.Services.HistoryParse.Processing.Parsing;
using CSGOStats.Services.HistoryParse.Runtime;
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
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
            };
#if DEBUG
            var environment = Environments.Development;
#else
            var environment = Environments.Production;
#endif
            var startupBuilder = await Startup
                .ForEnvironment(Service.Name, environment)
                .WithJobsAsync(ScheduleExtensions.ConfigureJobsAsync);
            await startupBuilder
                .WithMessaging<Service>()
                .UsesPostgres<HistoryParseContext>()
                .ConfigureServices(ConfigureServiceProvider)
                .RunAsync(new HistoryParseRuntimeAction());
        }

        private static IServiceCollection ConfigureServiceProvider(this IServiceCollection services, IConfigurationRoot configuration) =>
            services
                .AddScoped<Processor>()
                .AddScoped<IPageParser<HistoryPageModel>, Parser>()
                .ConfigureMatchSettings(configuration)
                .ConfigureDatabase()
                .ConfigureThrottling(configuration);

        private static IServiceCollection ConfigureDatabase(this IServiceCollection services) =>
            services
                .RegisterPostgresRepositoryFor<ParsedMatch>()
                .AddScoped<Upsert<ParsedMatch, Guid>>()
                .AddScoped<IEntityFactory<ParsedMatch, Guid>, ParsedMatchFactory>();

        private static IServiceCollection ConfigureThrottling(this IServiceCollection services, IConfigurationRoot configuration) =>
            services.AddThrottlingFromConfiguration(configuration, "Jobs:Common");
    }
}
