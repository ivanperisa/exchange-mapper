namespace ExchangeMapper.Domain.Entities;

public class StudyProgram
{
    public Guid Id { get; set; }
    public Guid InstitutionId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;
    public string IscedCode { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    public Institution Institution { get; set; } = null!;
    public ICollection<StudyProfile> StudyProfiles { get; set; } = [];
}
