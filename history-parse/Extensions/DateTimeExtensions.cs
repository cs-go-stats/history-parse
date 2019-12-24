using NodaTime;

namespace CSGOStats.Services.HistoryParse.Extensions
{
    // todo: to extensions
    public static class DateTimeExtensions
    {
        public static OffsetDate UtcDate(this LocalDate x) => new OffsetDate(x, Offset.Zero);

        public static bool GreaterThan(this OffsetDate x, OffsetDate y) => x.Date > y.Date;
    }
}