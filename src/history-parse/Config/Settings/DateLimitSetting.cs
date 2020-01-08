using NodaTime;

namespace CSGOStats.Services.HistoryParse.Config.Settings
{
    public class DateLimitSetting
    {
        public OffsetDate LowerBound { get; }

        public DateLimitSetting(OffsetDate lowerBound)
        {
            LowerBound = lowerBound;
        }
    }
}