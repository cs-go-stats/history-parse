using CSGOStats.Infrastructure.Validation;

namespace CSGOStats.Services.HistoryParse.Config.Settings
{
    public class CycleSetting
    {
        public string CronExpression { get; }

        public CycleSetting(string cronExpression)
        {
            CronExpression = cronExpression.NotNull(nameof(cronExpression));
        }
    }
}