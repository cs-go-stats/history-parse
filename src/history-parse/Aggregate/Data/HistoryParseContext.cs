using CSGOStats.Infrastructure.DataAccess.Contexts.EF;
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
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(Entities.ParsedMatch).Assembly);
    }

    public class MigrationsDataContext : HistoryParseContext
    {
        public MigrationsDataContext()
            : base(new PostgreConnectionSettings(
                host: "localhost",
                database: "history-parse",
                username: "postgres",
                password: "dotFive1",
                isAuditEnabled: false))
        {
        }
    }
}