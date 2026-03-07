namespace ExchangeMapper.Application.DTOs.Requests;

public class NewStudyProfileRequestDto
{
    public Guid StudyProgramId { get; set; }
    public string ProfileName { get; set; } = string.Empty;
    public string? ProfileNameEn { get; set; }
}
