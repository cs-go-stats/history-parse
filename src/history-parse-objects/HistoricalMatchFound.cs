using System;
using CSGOStats.Infrastructure.Core.Communication.Payload;
using CSGOStats.Infrastructure.Core.Validation;

namespace CSGOStats.Services.HistoryParse.Objects
{
    public class HistoricalMatchFound : IMessage
    {
        public Guid MatchId { get; }

        public string Link { get; }

        public int Stars { get; }

        public HistoricalMatchFound(Guid matchId, in string link, int stars)
        {
            MatchId = matchId.AnythingBut(Guid.Empty, nameof(matchId));
            Link = link.NotNull(link);
            Stars = stars.Natural(nameof(stars));
        }
    }
}