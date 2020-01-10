using System;
using CSGOStats.Services.Core.Handling.Entities;
using CSGOStats.Services.HistoryParse.Aggregate.Entities;

namespace CSGOStats.Services.HistoryParse.Aggregate.Factories
{
    public class ParsedMatchFactory : IEntityFactory<ParsedMatch, Guid>
    {
        public ParsedMatch CreateEmpty(Guid id) => new ParsedMatch(id, 1L);
    }
}