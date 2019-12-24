using System.Threading.Tasks;
using CSGOStats.Infrastructure.DataAccess.Contexts;
using CSGOStats.Infrastructure.DataAccess.Repositories;
using CSGOStats.Infrastructure.Extensions;
using CSGOStats.Infrastructure.Messaging.Transport;
using CSGOStats.Infrastructure.PageParse.Page;
using CSGOStats.Services.HistoryParse.Aggregate;
using CSGOStats.Services.HistoryParse.Aggregate.Data;
using CSGOStats.Services.HistoryParse.Config;
using CSGOStats.Services.HistoryParse.Config.Settings;
using CSGOStats.Services.HistoryParse.Processing;
using CSGOStats.Services.HistoryParse.Processing.Page.Model.State;
using CSGOStats.Services.HistoryParse.Processing.Parsing;
using CSGOStats.Services.HistoryParse.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace CSGOStats.Services.HistoryParse
{
    internal static class Program
    {
        private static async Task Main()
        {
            var configuration = CreateConfiguration();
            var provider = CreateServiceProvider(configuration);

            try
            {
                await provider.GetService<BaseDataContext>().Database.EnsureCreatedAsync();
                await provider.GetService<Processor>().RunAsync();
            }
            finally
            {
                await provider.DisposeAsync();
            }
        }

        private static ServiceProvider CreateServiceProvider(IConfigurationRoot configuration)
        {
            // todo: service collection to core lib
            return new ServiceCollection()
                .AddScoped<Processor>()
                .AddScoped<IDataLoader, HttpDataLoader>()
                .AddScoped<IPageParser<HistoryPageModel>, Parser>()
                .AddScoped<IEventBus, RabbitMqEventBus>()
                .ConfigureRabbitMqConnectionSetting(configuration)
                .ConfigureMatchSettings(configuration)
                .ConfigureLogging(configuration)
                .ConfigureDatabase(configuration)
                .AddScoped<AggregateFacade>()
                .BuildServiceProvider();
        }

        // todo: configuration to core lib
        private static IConfigurationRoot CreateConfiguration() => new ConfigurationBuilder()
#if DEBUG
            .AddJsonFile($"appsettings.Debug.json", optional: false, reloadOnChange: true)
#else
            .AddJsonFile($"appsettings.Production.json", optional: false, reloadOnChange: true)
#endif
            .AddEnvironmentVariables()
            .Build();

        private static IServiceCollection ConfigureLogging(this IServiceCollection services, IConfigurationRoot configuration) =>
            services.AddLogging(builder =>
            {
                var setting = configuration.GetFromConfiguration(
                    sectionName: "Logging",
                    creatingFunctor: configurationSection => new LogSetting(
                        messageTemplate: configurationSection["MessageTemplate"],
                        filename: configurationSection["Filename"],
                        fileSizeLimit: configurationSection["FileSizeLimit"].Long()));
                builder
                    .AddSerilog(
                        new LoggerConfiguration()
                            .Enrich.FromLogContext()
                            .MinimumLevel.Debug()
                            .WriteTo.ColoredConsole(
                                outputTemplate: setting.MessageTemplate)
                            .WriteTo.File(
                                path: setting.Filename,
                                fileSizeLimitBytes: setting.FileSizeLimit,
                                outputTemplate: setting.MessageTemplate)
                            .CreateLogger());
            });

        private static IServiceCollection ConfigureDatabase(this IServiceCollection services, IConfigurationRoot configuration) =>
            services
                .ConfigurePostgresConnection(configuration) // todo: to core package
                .AddScoped<BaseDataContext, HistoryParseContext>()
                .AddScoped<IRepository<Aggregate.Entities.HistoryParse>, HistoryParseRepository>();
    }
}
