using System.Threading.Tasks;
using CSGOStats.Infrastructure.PageParse.Page.Loading;
using CSGOStats.Services.HistoryParse.Processing.Parsing;
using Xunit;

namespace history_parse_tests
{
    public class HistoryParseTests
    {
        [Fact]
        public Task HistoryMainPageParseTest()
        {
            return new Parser().ParseAsync(new FileContentLoader("Pages/results-page-v1.htm"));
        }
    }
}