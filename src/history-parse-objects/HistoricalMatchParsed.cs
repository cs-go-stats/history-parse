using CSGOStats.Extensions.Validation;
using CSGOStats.Infrastructure.Messaging.Payload;

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
            Link = link.NotNull(nameof(link));
            Team1 = team1.NotNull(nameof(team1));
            Team2 = team2.NotNull(nameof(team2));
            Event = @event.NotNull(nameof(@event));
            Stars = stars.Natural(nameof(stars));
        }
    }
}