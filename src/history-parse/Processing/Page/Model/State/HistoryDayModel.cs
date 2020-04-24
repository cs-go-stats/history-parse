using System.Diagnostics;
using CSGOStats.Infrastructure.Core.PageParse.Page.Parse;
using CSGOStats.Infrastructure.Core.PageParse.Page.Structure.Containers;
using CSGOStats.Infrastructure.Core.PageParse.Page.Structure.Markers;
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