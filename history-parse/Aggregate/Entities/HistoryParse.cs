using System;
using CSGOStats.Infrastructure.DataAccess.Entities;

namespace CSGOStats.Services.HistoryParse.Aggregate.Entities
{
    public class HistoryParse : AggregateRoot, IHaveIdEntity
    {
        public Guid LastProcessedMatchId { get; private set; }

        public HistoryParse() { }

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