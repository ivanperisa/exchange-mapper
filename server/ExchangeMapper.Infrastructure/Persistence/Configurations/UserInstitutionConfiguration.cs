using ExchangeMapper.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExchangeMapper.Infrastructure.Persistence.Configurations;

public class UserInstitutionConfiguration : IEntityTypeConfiguration<UserInstitution>
{
    public void Configure(EntityTypeBuilder<UserInstitution> builder)
    {
        builder.ToTable("user_institution");

        builder.HasKey(ui => ui.Id);

        builder.Property(ui => ui.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");
        builder.Property(ui => ui.UserId).HasColumnName("user_id").IsRequired();
        builder.Property(ui => ui.InstitutionId).HasColumnName("institution_id").IsRequired();
        builder.Property(ui => ui.StudyProfileId).HasColumnName("study_profile_id");
        builder.Property(ui => ui.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("NOW()").IsRequired();

        builder.HasOne(ui => ui.User)
            .WithMany(u => u.UserInstitutions)
            .HasForeignKey(ui => ui.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ui => ui.Institution)
            .WithMany(i => i.UserInstitutions)
            .HasForeignKey(ui => ui.InstitutionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(ui => ui.StudyProfile)
            .WithMany(sp => sp.UserInstitutions)
            .HasForeignKey(ui => ui.StudyProfileId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(ui => new { ui.UserId, ui.InstitutionId, ui.StudyProfileId })
            .IsUnique();

        builder.HasMany(ui => ui.Exchanges)
            .WithOne(e => e.UserInstitution)
            .HasForeignKey(e => e.UserInstitutionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
