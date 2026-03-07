using ExchangeMapper.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExchangeMapper.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<UserInstitution> UserInstitutions => Set<UserInstitution>();
    public DbSet<Institution> Institutions => Set<Institution>();
    public DbSet<StudyProgram> StudyPrograms => Set<StudyProgram>();
    public DbSet<StudyProfile> StudyProfiles => Set<StudyProfile>();
    public DbSet<Exchange> Exchanges => Set<Exchange>();
    public DbSet<ExchangeCourse> ExchangeCourses => Set<ExchangeCourse>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<CourseMapping> CourseMappings => Set<CourseMapping>();
    public DbSet<MappingHistory> MappingHistories => Set<MappingHistory>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
