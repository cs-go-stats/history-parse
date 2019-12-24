using System.Threading.Tasks;

namespace CSGOStats.Services.HistoryParse.Utils
{
    // todo: should be a part of page-parse
    public interface IDataLoader
    {
        Task<string> GetRawAsync(string source);
    }
}