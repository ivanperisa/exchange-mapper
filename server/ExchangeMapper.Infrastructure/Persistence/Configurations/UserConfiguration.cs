using ExchangeMapper.Domain.Entities;
using ExchangeMapper.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExchangeMapper.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.ExternalId).HasColumnName("external_id").IsRequired();
        builder.Property(x => x.Email).HasColumnName("email").IsRequired();
        builder.Property(x => x.Name).HasColumnName("name").IsRequired();
        builder.Property(x => x.Role)
            .HasColumnName("role")
            .HasConversion<string>()
            .HasDefaultValue(UserRole.Student)
            .IsRequired();
        builder.Property(x => x.IsOnboarded).HasColumnName("is_onboarded").IsRequired();
        builder.Property(x => x.InstitutionId).HasColumnName("institution_id");
        builder.Property(x => x.StudyProfileId).HasColumnName("study_profile_id");
        builder.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();

        builder.HasOne(x => x.Institution)
            .WithMany()
            .HasForeignKey(x => x.InstitutionId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(x => x.StudyProfile)
            .WithMany(x => x.Users)
            .HasForeignKey(x => x.StudyProfileId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(x => x.ExternalId).IsUnique();
    }
}
