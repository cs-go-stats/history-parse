using System.Diagnostics;
using CSGOStats.Infrastructure.Core.PageParse.Page.Parse;
using CSGOStats.Infrastructure.Core.PageParse.Page.Structure.Containers;
using CSGOStats.Infrastructure.Core.PageParse.Page.Structure.Markers;

namespace CSGOStats.Services.HistoryParse.Processing.Page.Model.State
{
    [ModelRoot, RootContainer("*/body/div[@class = 'bgPadding']/div/div[@class = 'colCon']/div[@class = 'contentCol']/div/div[@class = 'results-holder']/div[@class = 'results-all']")]
    [DebuggerDisplay("Day groups: {Days.Length}.")]
    public class HistoryPageModel
    {
        internal static HistoryPageModel Default => new HistoryPageModel();

        [Collection, RequiredContainer("div[@class = 'results-sublist']")]
        public WrappedCollection<HistoryDayModel> Days { get; } = new WrappedCollection<HistoryDayModel>();
    }
}