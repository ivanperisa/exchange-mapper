namespace ExchangeMapper.Application.DTOs.Requests;

public class NewInstitutionRequestDto
{
    public string Name { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string? City { get; set; }
    public string? ErasmusCode { get; set; }
    public string IscedCode { get; set; } = string.Empty;
    public string ProgramName { get; set; } = string.Empty;
    public string ProfileName { get; set; } = string.Empty;
}
