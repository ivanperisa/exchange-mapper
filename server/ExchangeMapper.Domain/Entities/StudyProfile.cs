namespace ExchangeMapper.Domain.Entities;

public class StudyProfile
{
    public Guid Id { get; set; }
    public Guid StudyProgramId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    public StudyProgram StudyProgram { get; set; } = null!;
    public ICollection<UserInstitution> UserInstitutions { get; set; } = [];
    public ICollection<Course> Courses { get; set; } = [];
}
