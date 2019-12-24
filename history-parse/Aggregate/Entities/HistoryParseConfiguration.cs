using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CSGOStats.Services.HistoryParse.Aggregate.Entities
{
    internal class HistoryParseConfiguration : IEntityTypeConfiguration<HistoryParse>
    {
        public void Configure(EntityTypeBuilder<HistoryParse> builder)
        {
            builder.ToTable(nameof(HistoryParse), Service.Name); // todo: configure schema in shared package
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).IsRequired().ValueGeneratedNever();
            builder.Property(x => x.LastProcessedMatchId).IsRequired();
        }
    }
}