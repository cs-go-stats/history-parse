using System.Net.Http;
using System.Threading.Tasks;

namespace CSGOStats.Services.HistoryParse.Utils
{
    public class HttpDataLoader : IDataLoader
    {
        public async Task<string> GetRawAsync(string source)
        {
            var response = await new HttpClient().GetAsync(source).ContinueWith(x =>
                    x.Result.EnsureSuccessStatusCode(), TaskContinuationOptions.NotOnFaulted | TaskContinuationOptions.NotOnCanceled);
            return await response.Content.ReadAsStringAsync();
        }
    }
}