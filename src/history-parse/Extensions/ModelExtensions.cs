using System.Linq;
using CSGOStats.Services.HistoryParse.Processing.Page.Model.State;

namespace CSGOStats.Services.HistoryParse.Extensions
{
    public static class ModelExtensions
    {
        public static int StarRating(this MatchModel x) => x.Stars.Count();
    }
}