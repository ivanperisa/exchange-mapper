using ExchangeMapper.Application.DTOs.Responses;
using ExchangeMapper.Domain.Entities;

namespace ExchangeMapper.Application.Mappers;

public static class InstitutionMapper
{
    public static InstitutionResponseDto ToInstitutionDto(this Institution institution) => new()
    {
        Id = institution.Id,
        Name = institution.Name,
        NameEn = institution.NameEn,
        Country = institution.Country,
        City = institution.City,
        ErasmusCode = institution.ErasmusCode
    };

    public static StudyProgramResponseDto ToStudyProgramDto(this StudyProgram program) => new()
    {
        Id = program.Id,
        Name = program.Name,
        NameEn = program.NameEn,
        IscedCode = program.IscedCode
    };

    public static StudyProfileResponseDto ToStudyProfileDto(this StudyProfile profile) => new()
    {
        Id = profile.Id,
        Name = profile.Name,
        NameEn = profile.NameEn
    };

    public static UserInstitutionDto ToUserInstitutionDto(this UserInstitution ui) => new()
    {
        UserInstitutionId = ui.Id,
        HasActiveExchanges = ui.Exchanges.Count > 0,
        Institution = ui.Institution.ToInstitutionDto(),
        StudyProgram = ui.StudyProfile?.StudyProgram?.ToStudyProgramDto(),
        StudyProfile = ui.StudyProfile?.ToStudyProfileDto()
    };

    public static IEnumerable<InstitutionResponseDto> ToInstitutionDtos(this IEnumerable<Institution> institutions) =>
        institutions.Select(ToInstitutionDto);
}
