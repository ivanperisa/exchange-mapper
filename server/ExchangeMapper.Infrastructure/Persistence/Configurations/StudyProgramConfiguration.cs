using ExchangeMapper.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExchangeMapper.Infrastructure.Persistence.Configurations;

public class StudyProgramConfiguration : IEntityTypeConfiguration<StudyProgram>
{
    public void Configure(EntityTypeBuilder<StudyProgram> builder)
    {
        builder.ToTable("study_program");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.InstitutionId).HasColumnName("institution_id").IsRequired();
        builder.Property(x => x.Name).HasColumnName("name").IsRequired();
        builder.Property(x => x.IscedCode).HasColumnName("isced_code").IsRequired();

        builder.HasOne(x => x.Institution)
            .WithMany(x => x.StudyPrograms)
            .HasForeignKey(x => x.InstitutionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.StudyProfiles)
            .WithOne(x => x.StudyProgram)
            .HasForeignKey(x => x.StudyProgramId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
