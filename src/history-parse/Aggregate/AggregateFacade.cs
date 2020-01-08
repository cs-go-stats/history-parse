using System;
using System.Threading.Tasks;
using CSGOStats.Extensions.Extensions;
using CSGOStats.Extensions.Validation;
using CSGOStats.Infrastructure.DataAccess.Repositories;
using CSGOStats.Services.HistoryParse.Processing.Page.Model.State;

namespace CSGOStats.Services.HistoryParse.Aggregate
{
    public class AggregateFacade
    {
        private static readonly Guid AggregateId = Guid.Empty;

        private readonly IRepository<Entities.HistoryParse> _repository;

        public AggregateFacade(IRepository<Entities.HistoryParse> repository)
        {
            _repository = repository.NotNull(nameof(repository));
        }

        public async Task<Entities.HistoryParse> FindOrCreateAsync()
        {
            var entity = await _repository.FindAsync(AggregateId);
            if (entity != null)
            {
                return entity;
            }

            entity = new Entities.HistoryParse(AggregateId, 1L, Guid.Empty);
            await _repository.AddAsync(entity.Id, entity);

            return entity;
        }

        public async Task UpdateLastProcessedAsync(MatchModel match)
        {
            var entity = await _repository.GetAsync(AggregateId)
                .ContinueWith(x =>
                    x.Result.UpdateLastProcessedMatch(match.NotNull(nameof(match)).Link.Guid()));
            await _repository.UpdateAsync(entity.Id, entity);
        }
    }
}