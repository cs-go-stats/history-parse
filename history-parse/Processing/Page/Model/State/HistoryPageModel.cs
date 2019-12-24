using System.Diagnostics;
using CSGOStats.Infrastructure.PageParse.Page;
using CSGOStats.Infrastructure.PageParse.Structure.Containers;
using CSGOStats.Infrastructure.PageParse.Structure.Markers;

namespace CSGOStats.Services.HistoryParse.Processing.Page.Model.State
{
    [ModelRoot, RootContainer("*/body/div[@class = 'bgPadding']/div/div[@class = 'colCon']/div[@class = 'contentCol']/div/div[@class = 'results-holder']/div[@class = 'results-all']")]
    [DebuggerDisplay("Day groups: {System.Linq.Enumerable.Count(Days)}.")]
    // todo: count in wrapped collection
    public class HistoryPageModel
    {
        [Collection, RequiredContainer("div[@class = 'results-sublist']")]
        public WrappedCollection<HistoryDayModel> Days { get; } = new WrappedCollection<HistoryDayModel>();
    }
}