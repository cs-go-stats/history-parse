using System;
using System.Threading.Tasks;
using CSGOStats.Infrastructure.Core.Communication.Handling;
using CSGOStats.Infrastructure.Core.Communication.Transport;
using CSGOStats.Infrastructure.Core.Context;
using CSGOStats.Infrastructure.Core.Data.Storage;
using CSGOStats.Infrastructure.Core.Data.Storage.Repositories;
using CSGOStats.Infrastructure.Core.Validation;
using CSGOStats.Services.HistoryParse.Aggregate.Entities;
using CSGOStats.Services.HistoryParse.Config.Settings;
using CSGOStats.Services.HistoryParse.Objects;
using Microsoft.Extensions.Logging;

namespace CSGOStats.Services.HistoryParse.HistoricalMatchFound.Handlers
{
    internal class HistoryParseHandler : BaseMessageHandler<Objects.HistoricalMatchFound>
    {
        private readonly IEventBus _eventBus;
        private readonly IRepository<ParsedMatch> _historyParseRepository;
        private readonly HistoryParseLimitationSetting _limitationSetting;
        private readonly Upsert<ParsedMatch, Guid> _parsedMatchUpsert;
        private readonly ILogger<HistoryParseHandler> _logger;

        public HistoryParseHandler(
            ExecutionContext context, 
            IEventBus eventBus, 
            IRepository<ParsedMatch> historyParseRepository, 
            HistoryParseLimitationSetting limitationSetting, 
            Upsert<ParsedMatch, Guid> parsedMatchUpsert, 
            ILogger<HistoryParseHandler> logger) : base(context)
        {
            _eventBus = eventBus.NotNull(nameof(eventBus));
            _historyParseRepository = historyParseRepository.NotNull(nameof(historyParseRepository));
            _limitationSetting = limitationSetting.NotNull(nameof(limitationSetting));
            _parsedMatchUpsert = parsedMatchUpsert.NotNull(nameof(parsedMatchUpsert));
            _logger = logger.NotNull(nameof(logger));
        }

        public override async Task HandleAsync(Objects.HistoricalMatchFound message)
        {
            if (await PrecheckAsync(message) && await ProcessAsync(message))
            {
                await NotifyAsync(message);
            }
        }

        private async Task<bool> PrecheckAsync(Objects.HistoricalMatchFound message)
        {
            var entity = await _historyParseRepository.FindAsync(message.MatchId);
            if (entity == null)
            {
                return true;
            }

            _logger.LogInformation($"Reached already processed match row ('{message.MatchId}'). Running in 'no-force' mode. Processing has been terminated.");
            return false;
        }

        private async Task<bool> ProcessAsync(Objects.HistoricalMatchFound message)
        {
            await _parsedMatchUpsert.Async(message.MatchId, _ => { });

            if (message.Stars >= _limitationSetting.MinimumMatchRating)
            {
                return true;
            }

            _logger.LogDebug($"Match '{message.Link}' skipped due to star rating condition: '{message.Stars}'.");
            return false;

        }

        private Task NotifyAsync(in Objects.HistoricalMatchFound message)
        {
            _logger.LogInformation($"Publishing match parse event for: '{message.Link}'.");
            return _eventBus.PublishAsync(message: new HistoricalMatchParsed(message.MatchId, message.Link));
        }
    }
}