using System;
using CSGOStats.Infrastructure.Core.Data.Entities;
using NodaTime;

namespace CSGOStats.Services.HistoryParse.Aggregate.Entities
{
    public class ParsedMatch : AggregateRoot
    {
        public OffsetDateTime CreatedAt { get; private set; }

        public ParsedMatch() { /* EF */ }

        public ParsedMatch(Guid id, long version, OffsetDateTime createdAt)
            : base(id, version)
        {
            CreatedAt = createdAt;
        }
    }
}