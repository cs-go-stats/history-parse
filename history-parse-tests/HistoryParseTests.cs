using System.IO;
using System.Threading.Tasks;
using CSGOStats.Services.HistoryParse.Processing.Parsing;
using Xunit;

namespace history_parse_tests
{
    public class HistoryParseTests
    {
        [Fact]
        public async Task HistoryMainPageParseTest()
        {
            var content = await GetFileHtmlAsync("Pages/results-page-v1.htm");
            var model = await new Parser().ParseAsync(content);
        }

        private static async Task<string> GetFileHtmlAsync(string fileRelativePath)
        {
            await using var fileStream = new FileStream(fileRelativePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            using var reader = new StreamReader(fileStream);
            return await reader.ReadToEndAsync();
        }
    }
}