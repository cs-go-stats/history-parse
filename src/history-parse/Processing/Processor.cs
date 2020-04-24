using System;
using System.Linq;
using System.Threading.Tasks;
using CSGOStats.Infrastructure.Core.Communication.Transport;
using CSGOStats.Infrastructure.Core.Extensions;
using CSGOStats.Infrastructure.Core.PageParse.Page.Load;
using CSGOStats.Infrastructure.Core.PageParse.Page.Parse;
using CSGOStats.Infrastructure.Core.Throttling;
using CSGOStats.Infrastructure.Core.Validation;
using CSGOStats.Services.HistoryParse.Config.Settings;
using CSGOStats.Services.HistoryParse.Extensions;
using CSGOStats.Services.HistoryParse.Processing.Page.Model.State;
using Microsoft.Extensions.Logging;

namespace CSGOStats.Services.HistoryParse.Processing
{
    internal class Processor
    {
        private readonly IPageParser<HistoryPageModel> _pageParser;
        private readonly IEventBus _eventBus;
        private readonly ILogger<Processor> _logger;
        private readonly IThrottlingAgent _throttlingAgent;
        private readonly HistoryParseLimitationSetting _limitationSetting;

        public Processor(
            IPageParser<HistoryPageModel> pageParser,
            IEventBus eventBus,
            ILogger<Processor> logger,
            IThrottlingAgent throttlingAgent,
            HistoryParseLimitationSetting limitationSetting)
        {
            _pageParser = pageParser.NotNull(nameof(pageParser));
            _eventBus = eventBus.NotNull(nameof(eventBus));
            _logger = logger.NotNull(nameof(logger));
            _throttlingAgent = throttlingAgent.NotNull(nameof(throttlingAgent));
            _limitationSetting = limitationSetting.NotNull(nameof(limitationSetting));
        }

        public async Task RunAsync(bool isForcedRun)
        {
            var page = 0;

            do
            {
                var result = await ParseHistoricalPageAsync(page);

                foreach (var (day, match) in result.Days.SelectMany(d => d.Matches.Select(x => (d.Day, x))))
                {
                    if (_limitationSetting.MinimumMatchDate.GreaterThan(day))
                    {
                        _logger.LogInformation("Reached history lower bound. Processing has been terminated.");
                        return;
                    }

                    var matchId = match.Link.Guid();
                    await _eventBus.PublishAsync(message: new Objects.HistoricalMatchFound(matchId, match.Link, match.StarRating()));
                }

                if (!isForcedRun)
                {
                    return;
                }

                page++;

                await _throttlingAgent.ThrottleAsync();
            } while (true);
        }

        private async Task<HistoryPageModel> ParseHistoricalPageAsync(int page)
        {
            try
            {
                return await _pageParser.ParseAsync(GetHistoryPageLoader(page * 100));
            }
            catch (Exception e)
            {
                _logger.LogError(e, @"Error trying to parse historical page {page}.");
                return HistoryPageModel.Default;
            }
        }

        private static IContentLoader GetHistoryPageLoader(int offset) =>
            new HttpContentLoader($"https://www.hltv.org/results?offset={offset}");
    }
}