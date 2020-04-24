using System;
using CSGOStats.Infrastructure.Core.Data.Entities;
using CSGOStats.Infrastructure.Core.Extensions;
using CSGOStats.Services.HistoryParse.Aggregate.Entities;

namespace CSGOStats.Services.HistoryParse.Aggregate.Factories
{
    public class ParsedMatchFactory : IEntityFactory<ParsedMatch, Guid>
    {
        public ParsedMatch CreateEmpty(Guid id) => new ParsedMatch(id, 1L, DateTimeExtensions.GetCurrentDate);
    }
}