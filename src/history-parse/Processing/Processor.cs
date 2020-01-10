using System;
using System.Linq;
using System.Threading.Tasks;
using CSGOStats.Extensions.Extensions;
using CSGOStats.Extensions.Validation;
using CSGOStats.Infrastructure.DataAccess.Repositories;
using CSGOStats.Infrastructure.Messaging.Transport;
using CSGOStats.Infrastructure.PageParse.Page.Loading;
using CSGOStats.Infrastructure.PageParse.Page.Parsing;
using CSGOStats.Services.Core.Handling.Storage;
using CSGOStats.Services.HistoryParse.Aggregate.Entities;
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
        private readonly IRepository<ParsedMatch> _historyParseRepository;
        private readonly HistoryParseLimitationSetting _limitationSetting;
        private readonly Upsert<ParsedMatch, Guid> _parsedMatchUpsert;

        public Processor(
            IPageParser<HistoryPageModel> pageParser,
            IEventBus eventBus,
            ILogger<Processor> logger,
            IRepository<ParsedMatch> historyParseRepository,
            HistoryParseLimitationSetting limitationSetting, 
            Upsert<ParsedMatch, Guid> parsedMatchUpsert)
        {
            _pageParser = pageParser.NotNull(nameof(pageParser));
            _eventBus = eventBus.NotNull(nameof(eventBus));
            _logger = logger.NotNull(nameof(logger));
            _historyParseRepository = historyParseRepository.NotNull(nameof(historyParseRepository));
            _limitationSetting = limitationSetting.NotNull(nameof(limitationSetting));
            _parsedMatchUpsert = parsedMatchUpsert.NotNull(nameof(parsedMatchUpsert));
        }

        public async Task RunAsync(bool isForcedRun)
        {
            var page = 0;

            do
            {
                var result = await _pageParser.ParseAsync(GetHistoryPageLoader(page * 100)); // todo catch exception here

                foreach (var (day, match) in result.Days.SelectMany(d => d.Matches.Select(x => (d.Day, x))))
                {
                    var matchId = match.Link.Guid();
                    if (!await ContinueToProcessAsync(matchId, day, isForcedRun))
                    {
                        return;
                    }

                    await ProcessMatchAsync(matchId, match);
                }

                page++;
            } while (true);
        }

        private async Task<bool> ContinueToProcessAsync(Guid matchId, OffsetDate matchDate, bool isForcedRun)
        {
            if (_limitationSetting.MinimumMatchDate.GreaterThan(matchDate))
            {
                _logger.LogInformation("Reached history lower bound. Processing has been terminated.");
                return false;
            }

            if (isForcedRun)
            {
                return true;
            }

            var entity = await _historyParseRepository.FindAsync(matchId);
            if (entity != null)
            {
                return true;
            }

            _logger.LogInformation("Reached already processed match row. Running in 'no-force' mode. Processing has been terminated.");
            return false;
        }

        // todo: split method to: pre-check / process / notify
        private async Task ProcessMatchAsync(Guid matchId, MatchModel match)
        {
            var rating = match.StarRating();
            if (rating < _limitationSetting.MinimumMatchRating)
            {
                _logger.LogDebug($"Match '{match.Link}' skipped due to star rating condition: '{rating}'.");
                return;
            }

            await _parsedMatchUpsert.Async(
                matchId,
                _ => { });
            await NotifyMatchParsedAsync(match);
        }

        private Task NotifyMatchParsedAsync(MatchModel match)
        {
            _logger.LogInformation($"Publishing match parse event for: '{match.Link}'.");
            return _eventBus.PublishAsync(message: HistoricalMatchParsedFactory.Create(match));
        }

        private static IContentLoader GetHistoryPageLoader(int offset) =>
            new HttpContentLoader($"https://www.hltv.org/results?offset={offset}");
    }
}