namespace ExchangeMapper.Domain.Entities;

public class MappingHistory
{
    public Guid Id { get; set; }
    public Guid CourseMappingId { get; set; }
    public Guid ChangedBy { get; set; }
    public string Snapshot { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    public CourseMapping CourseMapping { get; set; } = null!;
    public User ChangedByUser { get; set; } = null!;
}
