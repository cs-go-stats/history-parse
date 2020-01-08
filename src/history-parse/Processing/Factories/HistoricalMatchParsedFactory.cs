using CSGOStats.Services.Core.Links;
using CSGOStats.Services.HistoryParse.Extensions;
using CSGOStats.Services.HistoryParse.Objects;
using CSGOStats.Services.HistoryParse.Processing.Page.Model.State;

namespace CSGOStats.Services.HistoryParse.Processing.Factories
{
    public static class HistoricalMatchParsedFactory
    {
        public static HistoricalMatchParsed Create(MatchModel model) =>
            new HistoricalMatchParsed(
                model.Link.HltvUri(),
                model.Team1,
                model.Team2,
                model.Event,
                model.StarRating());
    }
}