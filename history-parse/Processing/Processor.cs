using System.Linq;
using System.Threading.Tasks;
using CSGOStats.Infrastructure.Messaging.Transport;
using CSGOStats.Infrastructure.PageParse.Page;
using CSGOStats.Infrastructure.Validation;
using CSGOStats.Services.HistoryParse.Aggregate;
using CSGOStats.Services.HistoryParse.Config.Settings;
using CSGOStats.Services.HistoryParse.Extensions;
using CSGOStats.Services.HistoryParse.Objects;
using CSGOStats.Services.HistoryParse.Processing.Page.Model.State;
using CSGOStats.Services.HistoryParse.Utils;
using Microsoft.Extensions.Logging;
using NodaTime;

namespace CSGOStats.Services.HistoryParse.Processing
{
    internal class Processor
    {
        private readonly IDataLoader _dataLoader;
        private readonly IPageParser<HistoryPageModel> _pageParser;
        private readonly IEventBus _eventBus;
        private readonly ILogger<Processor> _logger;
        private readonly DateLimitSetting _dateLimitSetting;
        private readonly MatchStarSetting _matchStarSetting;
        private readonly AggregateFacade _aggregateFacade;

        public Processor(
            IDataLoader dataLoader,
            IPageParser<HistoryPageModel> pageParser,
            IEventBus eventBus,
            ILogger<Processor> logger,
            DateLimitSetting dateLimitSetting,
            MatchStarSetting matchStarSetting,
            AggregateFacade aggregateFacade)
        {
            _dataLoader = dataLoader.NotNull(nameof(dataLoader));
            _pageParser = pageParser.NotNull(nameof(pageParser));
            _eventBus = eventBus.NotNull(nameof(eventBus));
            _logger = logger.NotNull(nameof(logger));
            _dateLimitSetting = dateLimitSetting.NotNull(nameof(dateLimitSetting));
            _matchStarSetting = matchStarSetting.NotNull(nameof(matchStarSetting));
            _aggregateFacade = aggregateFacade;
        }

        public async Task RunAsync()
        {
            var page = 0;
            var (aggregate, aggregateUpdated) = (await _aggregateFacade.FindOrCreateAsync(), false);
            do
            {
                var content = await GetHistoryPageAsync(page * 100);
                var result = await _pageParser.ParseAsync(content); // todo catch exception here
                
                foreach (var (day, match, index) in result.Days.SelectMany(d => d.Matches.Select((x, i) => (d.Day, x, i))))
                {
                    if (!await ProcessMatchAsync(day, match, aggregate))
                    {
                        return;
                    }

                    if (index != 0 || aggregateUpdated)
                    {
                        continue;
                    }

                    await _aggregateFacade.UpdateLastProcessedAsync(match);
                    aggregateUpdated = true;
                }

                page++;
            } while (true);
        }

        private Task<string> GetHistoryPageAsync(int offset)
        {
            _logger.LogInformation($"Requesting history with offset: {offset}.");
            return _dataLoader.GetRawAsync($"https://www.hltv.org/results?offset={offset}");
        }

        // todo: split method to: pre-check / process / notify
        private async Task<bool> ProcessMatchAsync(OffsetDate date, MatchModel match, Aggregate.Entities.HistoryParse aggregate)
        {
            if (aggregate.LastProcessedMatchId == match.Link.Guid())
            {
                _logger.LogDebug($"Match '{match.Link}' was already parsed. Processing has been terminated.");
                return false;
            }

            if (_dateLimitSetting.LowerBound.GreaterThan(date))
            {
                _logger.LogInformation("Reached history lower bound. Processing has been terminated.");
                return false;
            }

            var rating = match.StarRating();
            if (rating < _matchStarSetting.LowerBound)
            {
                _logger.LogDebug($"Match '{match.Link}' skipped due to star rating condition: '{rating}'.");
                return true;
            }

            _logger.LogInformation($"Publishing match parse event for: '{match.Link}'.");
            await NotifyMatchParsedAsync(match);
            return true;
        }

        private Task NotifyMatchParsedAsync(MatchModel match) => _eventBus
            .PublishAsync(
                new HistoricalMatchParsed(
                    match.Link,
                    match.Team1,
                    match.Team2,
                    match.Event,
                    match.StarRating()));
    }
}