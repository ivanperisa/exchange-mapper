using ExchangeMapper.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExchangeMapper.Infrastructure.Persistence.Configurations;

public class InstitutionConfiguration : IEntityTypeConfiguration<Institution>
{
    public void Configure(EntityTypeBuilder<Institution> builder)
    {
        builder.ToTable("institution");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.Name).HasColumnName("name").IsRequired();
        builder.Property(x => x.Country).HasColumnName("country").IsRequired();
        builder.Property(x => x.City).HasColumnName("city");
        builder.Property(x => x.ErasmusCode).HasColumnName("erasmus_code");
        builder.Property(x => x.IsHome).HasColumnName("is_home").IsRequired();

        builder.HasMany(x => x.StudyPrograms)
            .WithOne(x => x.Institution)
            .HasForeignKey(x => x.InstitutionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
