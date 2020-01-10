using System;
using CSGOStats.Services.Core.Handling.Entities;

namespace CSGOStats.Services.HistoryParse.Aggregate.Entities
{
    public class ParsedMatch : AggregateRoot
    {
        public ParsedMatch(Guid id, long version)
            : base(id, version)
        {
        }
    }
}