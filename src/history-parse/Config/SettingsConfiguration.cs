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
                            sectionName: "HistoryParseLimitations",
                            creatingFunctor: configurationSection =>
                            {
                                var dateBoundSection = configurationSection.GetSection("MatchDateLowerBound");
                                return new HistoryParseLimitationSetting(
                                    minimumMatchDate: new LocalDate(
                                        year: dateBoundSection["Year"].Int(),
                                        month: dateBoundSection["Month"].Int(),
                                        day: dateBoundSection["Day"].Int()).UtcDate(),
                                    minimumMatchRating: configurationSection.GetSection("MatchStars")["LowerBound"].Int());
                            }));
    }
}