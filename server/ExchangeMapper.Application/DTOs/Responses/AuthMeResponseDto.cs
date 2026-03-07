namespace ExchangeMapper.Application.DTOs.Responses;

public class AuthMeResponseDto
{
    public bool IsAuthenticated { get; set; }
    public string? Sub { get; set; }
    public string? Email { get; set; }
    public string? Name { get; set; }
    public string? Role { get; set; }
    public bool IsOnboarded { get; set; }
    public List<UserInstitutionDto> Institutions { get; set; } = [];
}
