using ExchangeMapper.Domain.Enums;

namespace ExchangeMapper.Application.DTOs.Requests;

public class OnboardingRequestDto
{
    public UserRole Role { get; set; }
    public List<InstitutionEntryDto> Institutions { get; set; } = [];
}
