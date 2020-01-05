using System;
using CSGOStats.Services.Core.Handling.Entities;

namespace CSGOStats.Services.HistoryParse.Aggregate.Entities
{
    public class HistoryParse : AggregateRoot
    {
        public Guid LastProcessedMatchId { get; private set; }

        public HistoryParse(Guid id, long version, Guid lastProcessedMatchId) 
            : base(id, version)
        {
            LastProcessedMatchId = lastProcessedMatchId;
        }

        public HistoryParse UpdateLastProcessedMatch(Guid lastProcessedMatchId)
        {
            LastProcessedMatchId = lastProcessedMatchId;
            Update();
            return this;
        }
    }
}