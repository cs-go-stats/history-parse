using CSGOStats.Infrastructure.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CSGOStats.Services.HistoryParse.Aggregate.Entities
{
    internal class ParsedMatchConfiguration : IEntityTypeConfiguration<ParsedMatch>
    {
        public void Configure(EntityTypeBuilder<ParsedMatch> builder)
        {
            builder.RegisterTable(Service.Name);
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).IsRequired().ValueGeneratedNever();
            builder.Property(x => x.CreatedAt).OffsetDateTime().HasDefaultValue(DateTimeExtensions.GetCurrentDate);
        }
    }
}