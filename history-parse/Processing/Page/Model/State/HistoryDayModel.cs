using System.Diagnostics;
using CSGOStats.Infrastructure.PageParse.Page.Parsing;
using CSGOStats.Infrastructure.PageParse.Structure.Containers;
using CSGOStats.Infrastructure.PageParse.Structure.Markers;
using CSGOStats.Services.HistoryParse.Processing.Mappings.Attributes;
using NodaTime;

namespace CSGOStats.Services.HistoryParse.Processing.Page.Model.State
{
    [ModelLeaf]
    [DebuggerDisplay("Matches in group: {System.Linq.Enumerable.Count(Matches)}")]
    public class HistoryDayModel
    {
        [RequiredContainer("span"), GroupDateValue] 
        public OffsetDate Day { get; private set; }

        [Collection, RequiredContainer("div[@class = 'result-con']")] 
        public WrappedCollection<MatchModel> Matches { get; } = new WrappedCollection<MatchModel>();
    }
}