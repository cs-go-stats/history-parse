using CSGOStats.Infrastructure.DataAccess.Contexts;
using CSGOStats.Infrastructure.DataAccess.Repositories;

namespace CSGOStats.Services.HistoryParse.Aggregate.Data
{
    public class HistoryParseRepository : EfRepository<Entities.HistoryParse>
    {
        public HistoryParseRepository(BaseDataContext context) 
            : base(context)
        {
        }
    }
}