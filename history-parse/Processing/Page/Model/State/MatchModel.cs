using CSGOStats.Infrastructure.PageParse.Page;
using CSGOStats.Infrastructure.PageParse.Structure.Containers;
using CSGOStats.Infrastructure.PageParse.Structure.Markers;
using CSGOStats.Services.HistoryParse.Processing.Mappings.Attributes;

namespace CSGOStats.Services.HistoryParse.Processing.Page.Model.State
{
    [ModelLeaf]
    public class MatchModel
    {
        [RequiredContainer("a"), AnchorLinkValue]
        public string Link { get; private set; }

        [RequiredContainer("a/div/table/tr/td[@class = 'team-cell'][1]/div/img"), ImageTitleValue]
        public string Team1 { get; private set; }

        [RequiredContainer("a/div/table/tr/td[@class = 'team-cell'][2]/div/img"), ImageTitleValue]
        public string Team2 { get; private set; }

        [RequiredContainer("a/div/table/tr/td[@class = 'event']/img"), ImageTitleValue]
        public string Event { get; private set; }

        [Collection, OptionalContainer("a/div/table/tr/td[@class = 'star-cell']/div[@class = 'map-and-stars']/div[@class = 'stars']/i")]
        public WrappedCollection<StarModel> Stars { get; } = new WrappedCollection<StarModel>();
    }
}