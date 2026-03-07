using ExchangeMapper.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExchangeMapper.Infrastructure.Persistence.Configurations;

public class ExchangeCourseConfiguration : IEntityTypeConfiguration<ExchangeCourse>
{
    public void Configure(EntityTypeBuilder<ExchangeCourse> builder)
    {
        builder.ToTable("exchange_course");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");
        builder.Property(x => x.ExchangeId).HasColumnName("exchange_id").IsRequired();
        builder.Property(x => x.Name).HasColumnName("name").IsRequired();
        builder.Property(x => x.NameEn).HasColumnName("name_en").IsRequired();
        builder.Property(x => x.Ects).HasColumnName("ects").IsRequired();
        builder.Property(x => x.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("NOW()").IsRequired();

        builder.HasOne(x => x.Exchange)
            .WithMany(x => x.ExchangeCourses)
            .HasForeignKey(x => x.ExchangeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
