namespace ExchangeMapper.Domain.Entities;

public class StudyProfile
{
    public Guid Id { get; set; }
    public Guid StudyProgramId { get; set; }
    public string Name { get; set; } = string.Empty;

    public StudyProgram StudyProgram { get; set; } = null!;
    public ICollection<User> Users { get; set; } = new List<User>();
}
