namespace ExchangeMapper.Domain.Entities;

public class Course
{
    public Guid Id { get; set; }
    public Guid StudyProfileId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;
    public decimal Ects { get; set; }
    public DateTime CreatedAt { get; set; }

    public StudyProfile StudyProfile { get; set; } = null!;
    public ICollection<CourseMapping> CourseMappings { get; set; } = [];
}
