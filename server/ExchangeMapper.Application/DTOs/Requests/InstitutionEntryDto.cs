namespace ExchangeMapper.Application.DTOs.Requests;

public class InstitutionEntryDto
{
    public Guid? ExistingStudyProfileId { get; set; }
    public Guid? ExistingInstitutionId { get; set; }
    public NewStudyProfileRequestDto? NewStudyProfile { get; set; }
    public NewInstitutionRequestDto? NewInstitution { get; set; }
}
