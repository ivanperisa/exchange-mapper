using ExchangeMapper.Domain.Enums;

namespace ExchangeMapper.Application.DTOs;

public class OnboardingRequestDto
{
    public UserRole Role { get; set; }
    public Guid? ExistingInstitutionId { get; set; }
    public NewInstitutionDto? NewInstitution { get; set; }
    public Guid? ExistingStudyProfileId { get; set; }
    public NewStudyProfileDto? NewStudyProfile { get; set; }
}
