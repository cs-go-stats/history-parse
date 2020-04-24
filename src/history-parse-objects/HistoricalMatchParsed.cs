using System;
using CSGOStats.Infrastructure.Core.Communication.Payload;
using CSGOStats.Infrastructure.Core.Validation;

namespace CSGOStats.Services.HistoryParse.Objects
{
    public class HistoricalMatchParsed : IMessage
    {
        public Guid MatchId { get; }

        public string Link { get; }

        public HistoricalMatchParsed(Guid matchId, in string link)
        {
            MatchId = matchId.AnythingBut(Guid.Empty, nameof(link));
            Link = link.NotNull(nameof(link));
        }
    }
}