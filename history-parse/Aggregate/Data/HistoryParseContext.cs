using CSGOStats.Infrastructure.DataAccess.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CSGOStats.Services.HistoryParse.Aggregate.Data
{
    public class HistoryParseContext : BaseDataContext
    {
        public HistoryParseContext(PostgreConnectionSettings settings) 
            : base(settings)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) =>
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(Entities.HistoryParse).Assembly);
    }
}