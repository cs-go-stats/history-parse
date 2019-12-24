using System;
using CSGOStats.Infrastructure.DataAccess.Contexts;
using CSGOStats.Infrastructure.Extensions;
using CSGOStats.Infrastructure.Messaging.Transport;
using CSGOStats.Infrastructure.Validation;
using CSGOStats.Services.HistoryParse.Config.Settings;
using CSGOStats.Services.HistoryParse.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NodaTime;

namespace CSGOStats.Services.HistoryParse.Config
{
    public static class SettingsConfiguration
    {
        public static IServiceCollection ConfigureRabbitMqConnectionSetting(
           this IServiceCollection serviceProvider,
           IConfigurationRoot configuration) =>
               serviceProvider.AddSingleton(_ =>
                   configuration.GetFromConfiguration(
                       sectionName: "RabbitMqConnection",
                       creatingFunctor: configurationSection => 
                           new RabbitMqConnectionConfiguration(
                               host: configurationSection["Host"],
                               port: configurationSection["Port"].Int(),
                               username: configurationSection["Username"],
                               password: configurationSection["Password"],
                               heartbeat: configurationSection["Heartbeat"].Int())));

        public static IServiceCollection ConfigureMatchSettings(
            this IServiceCollection serviceProvider,
            IConfigurationRoot configuration) =>
                serviceProvider
                    .AddSingleton(_ => 
                        configuration.GetFromConfiguration(
                            sectionName: "MatchDateLowerBound",
                            creatingFunctor: configurationSection => new DateLimitSetting(
                                lowerBound: new LocalDate(
                                    year: configurationSection["Year"].Int(),
                                    month: configurationSection["Month"].Int(),
                                    day: configurationSection["Day"].Int()).UtcDate())))
                    .AddSingleton(_ => 
                        configuration.GetFromConfiguration(
                            sectionName: "MatchStars",
                            creatingFunctor: configurationSection => new MatchStarSetting(
                                lowerBound: configurationSection["LowerBound"].Int())));

        public static TSetting GetFromConfiguration<TSetting>(
            this IConfigurationRoot configuration,
            string sectionName,
            Func<IConfiguration, TSetting> creatingFunctor)
                where TSetting : class
        {
            var section = configuration.NotNull(nameof(configuration)).GetSection(sectionName.NotNull(nameof(sectionName)));
            return creatingFunctor.NotNull(nameof(creatingFunctor)).Invoke(section);
        }

        public static IServiceCollection ConfigurePostgresConnection(
            this IServiceCollection serviceProvider,
            IConfigurationRoot configuration) =>
                serviceProvider
                    .AddSingleton(_ =>
                        configuration.GetFromConfiguration(
                            sectionName: "PostgresConnection",
                            creatingFunctor: configurationSection => new PostgreConnectionSettings(
                                host: configurationSection["Host"],
                                database: configurationSection["Database"],
                                username: configurationSection["Username"],
                                password: configurationSection["Password"])));
    }
}