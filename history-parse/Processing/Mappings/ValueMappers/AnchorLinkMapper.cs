using CSGOStats.Infrastructure.PageParse.Mapping;
using HtmlAgilityPack;

namespace CSGOStats.Services.HistoryParse.Processing.Mappings.ValueMappers
{
    internal class AnchorLinkMapper : IValueMapper
    {
        public object Map(HtmlNode root) => root.GetAttributeValue("href", null);
    }
}