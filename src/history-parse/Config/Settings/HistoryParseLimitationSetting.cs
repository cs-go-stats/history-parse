using CSGOStats.Extensions.Validation;
using NodaTime;

namespace CSGOStats.Services.HistoryParse.Config.Settings
{
    public class HistoryParseLimitationSetting
    {
        public OffsetDate MinimumMatchDate { get; }

        public int MinimumMatchRating { get; }

        public HistoryParseLimitationSetting(OffsetDate minimumMatchDate, int minimumMatchRating)
        {
            MinimumMatchDate = minimumMatchDate;
            MinimumMatchRating = minimumMatchRating.Positive(nameof(minimumMatchRating));
        }
    }
}