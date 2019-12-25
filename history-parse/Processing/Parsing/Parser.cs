using CSGOStats.Infrastructure.PageParse.Mapping;
using CSGOStats.Infrastructure.PageParse.Page.Parsing;
using CSGOStats.Services.HistoryParse.Processing.Mappings;
using CSGOStats.Services.HistoryParse.Processing.Page.Model.State;

namespace CSGOStats.Services.HistoryParse.Processing.Parsing
{
    public class Parser : PageParser<HistoryPageModel>
    {
        public Parser()
            : base(new BaseDictionaryAdaptedValueMapperFactory(new HistoryPageParseMapperFactory()))
        {
        }
    }
}