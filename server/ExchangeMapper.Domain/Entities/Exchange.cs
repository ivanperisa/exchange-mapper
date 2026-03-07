namespace ExchangeMapper.Domain.Entities;

public class Exchange
{
    public Guid Id { get; set; }
    public Guid StudentId { get; set; }
    public Guid UserInstitutionId { get; set; }
    public Guid ForeignInstitutionId { get; set; }
    public string AcademicYear { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public User Student { get; set; } = null!;
    public UserInstitution UserInstitution { get; set; } = null!;
    public Institution ForeignInstitution { get; set; } = null!;
    public ICollection<ExchangeCourse> ExchangeCourses { get; set; } = [];
}
