using ExchangeMapper.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExchangeMapper.Infrastructure.Persistence.Configurations;

public class MappingHistoryConfiguration : IEntityTypeConfiguration<MappingHistory>
{
    public void Configure(EntityTypeBuilder<MappingHistory> builder)
    {
        builder.ToTable("mapping_history");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");
        builder.Property(x => x.CourseMappingId).HasColumnName("course_mapping_id").IsRequired();
        builder.Property(x => x.ChangedBy).HasColumnName("changed_by").IsRequired();
        builder.Property(x => x.Snapshot).HasColumnName("snapshot").HasColumnType("jsonb").IsRequired();
        builder.Property(x => x.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("NOW()").IsRequired();

        builder.HasOne(x => x.CourseMapping)
            .WithMany(x => x.MappingHistoryEntries)
            .HasForeignKey(x => x.CourseMappingId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.ChangedByUser)
            .WithMany(x => x.MappingHistoryChanges)
            .HasForeignKey(x => x.ChangedBy)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
