﻿using System.Linq;
using System.Threading.Tasks;
using CSGOStats.Extensions.Extensions;
using CSGOStats.Extensions.Validation;
using CSGOStats.Infrastructure.Messaging.Transport;
using CSGOStats.Infrastructure.PageParse.Page.Loading;
using CSGOStats.Infrastructure.PageParse.Page.Parsing;
using CSGOStats.Services.HistoryParse.Aggregate;
using CSGOStats.Services.HistoryParse.Config.Settings;
using CSGOStats.Services.HistoryParse.Extensions;
using CSGOStats.Services.HistoryParse.Processing.Factories;
using CSGOStats.Services.HistoryParse.Processing.Page.Model.State;
using Microsoft.Extensions.Logging;
using NodaTime;

namespace CSGOStats.Services.HistoryParse.Processing
{
    internal class Processor
    {
        private readonly IPageParser<HistoryPageModel> _pageParser;
        private readonly IEventBus _eventBus;
        private readonly ILogger<Processor> _logger;
        private readonly DateLimitSetting _dateLimitSetting;
        private readonly MatchStarSetting _matchStarSetting;
        private readonly AggregateFacade _aggregateFacade;

        public Processor(
            IPageParser<HistoryPageModel> pageParser,
            IEventBus eventBus,
            ILogger<Processor> logger,
            DateLimitSetting dateLimitSetting,
            MatchStarSetting matchStarSetting,
            AggregateFacade aggregateFacade)
        {
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
                var result = await _pageParser.ParseAsync(GetHistoryPageLoader(page * 100)); // todo catch exception here
                
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
            .PublishAsync(message: HistoricalMatchParsedFactory.Create(match));

        private static IContentLoader GetHistoryPageLoader(int offset) =>
            new HttpContentLoader($"https://www.hltv.org/results?offset={offset}");
    }
}