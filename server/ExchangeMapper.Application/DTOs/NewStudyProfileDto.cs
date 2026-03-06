namespace ExchangeMapper.Application.DTOs;

public class NewStudyProfileDto
{
    public Guid StudyProgramId { get; set; }
    public string ProfileName { get; set; } = string.Empty;
}
