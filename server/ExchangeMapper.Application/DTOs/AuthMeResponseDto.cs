namespace ExchangeMapper.Application.DTOs;

public class AuthMeResponseDto
{
    public bool IsAuthenticated { get; set; }
    public string Sub { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public bool IsOnboarded { get; set; }
    public InstitutionDto? Institution { get; set; }
    public StudyProgramDto? StudyProgram { get; set; }
    public StudyProfileDto? StudyProfile { get; set; }
}
