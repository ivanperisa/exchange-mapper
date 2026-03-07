namespace ExchangeMapper.Application.DTOs.Responses;

public class UserInstitutionDto
{
    public Guid UserInstitutionId { get; set; }
    public bool HasActiveExchanges { get; set; }
    public InstitutionResponseDto Institution { get; set; } = null!;
    public StudyProgramResponseDto? StudyProgram { get; set; }
    public StudyProfileResponseDto? StudyProfile { get; set; }
}
