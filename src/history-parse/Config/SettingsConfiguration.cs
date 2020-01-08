using CSGOStats.Extensions.Extensions;
using CSGOStats.Services.HistoryParse.Config.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NodaTime;

namespace CSGOStats.Services.HistoryParse.Config
{
    public static class SettingsConfiguration
    {
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
    }
}