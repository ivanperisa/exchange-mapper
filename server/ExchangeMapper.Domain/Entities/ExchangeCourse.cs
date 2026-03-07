namespace ExchangeMapper.Domain.Entities;

public class ExchangeCourse
{
    public Guid Id { get; set; }
    public Guid ExchangeId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;
    public decimal Ects { get; set; }
    public DateTime CreatedAt { get; set; }

    public Exchange Exchange { get; set; } = null!;
    public ICollection<CourseMapping> CourseMappings { get; set; } = [];
}
