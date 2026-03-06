namespace ExchangeMapper.Application.DTOs.Responses;

public class AuthMeResponseDto
{
    public bool IsAuthenticated { get; set; }
    public string Sub { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public bool IsOnboarded { get; set; }
    public InstitutionResponseDto? Institution { get; set; }
    public StudyProgramResponseDto? StudyProgram { get; set; }
    public StudyProfileResponseDto? StudyProfile { get; set; }
}
