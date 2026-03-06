using ExchangeMapper.Domain.Enums;

namespace ExchangeMapper.Application.DTOs.Requests;

public class CompleteOnboardingRequestDto
{
    public UserRole Role { get; set; }
    public Guid? ExistingInstitutionId { get; set; }
    public NewInstitutionRequestDto? NewInstitution { get; set; }
    public Guid? ExistingStudyProfileId { get; set; }
    public NewStudyProfileRequestDto? NewStudyProfile { get; set; }
}
