using System;
using CSGOStats.Infrastructure.Messaging.Payload;
using CSGOStats.Infrastructure.Validation;

namespace CSGOStats.Services.HistoryParse.Objects
{
    public class HistoricalMatchParsed : IMessage
    {
        public string Link { get; }

        public string Team1 { get; }

        public string Team2 { get; }

        public string Event { get; }

        public int Stars { get; }

        public HistoricalMatchParsed(
            string link,
            string team1,
            string team2,
            string @event,
            int stars)
        {
            Link = link.NotNull(nameof(link)).HltvUri();
            Team1 = team1.NotNull(nameof(team1));
            Team2 = team2.NotNull(nameof(team2));
            Event = @event.NotNull(nameof(@event));
            Stars = stars.Natural(nameof(stars));
        }
    }

    public static class Ext
    {
        private const string HltvRoot = "https://hltv.org";

        [Obsolete]
        public static string HltvUri(this string x) => HltvRoot + x;
    }
}