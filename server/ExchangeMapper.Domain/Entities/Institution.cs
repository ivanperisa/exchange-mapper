namespace ExchangeMapper.Domain.Entities;

public class Institution
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string? City { get; set; }
    public string? ErasmusCode { get; set; }
    public bool IsHome { get; set; }
    public DateTime CreatedAt { get; set; }

    public ICollection<StudyProgram> StudyPrograms { get; set; } = [];
    public ICollection<UserInstitution> UserInstitutions { get; set; } = [];
    public ICollection<Exchange> ForeignExchanges { get; set; } = [];
}
