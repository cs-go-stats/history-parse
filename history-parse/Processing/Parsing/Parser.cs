using CSGOStats.Infrastructure.PageParse.Mapping;
using CSGOStats.Infrastructure.PageParse.Page;
using CSGOStats.Services.HistoryParse.Processing.Mappings;
using CSGOStats.Services.HistoryParse.Processing.Page.Model.State;

namespace CSGOStats.Services.HistoryParse.Processing.Parsing
{
    public class Parser : PageParser<HistoryPageModel>
    {
        public Parser()
            // todo adapted factory to wrap only one factory
            : base(new BaseDictionaryAdaptedValueMapperFactory(
                new BaseDictionaryValueMapperFactory(), 
                new HistoryPageParseMapperFactory()))
        {
        }
    }
}