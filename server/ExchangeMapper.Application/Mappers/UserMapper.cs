using ExchangeMapper.Application.DTOs.Responses;
using ExchangeMapper.Domain.Entities;

namespace ExchangeMapper.Application.Mappers;

public static class UserMapper
{
    public static AuthMeResponseDto ToAuthMeResponseDto(this User user) => new()
    {
        IsAuthenticated = true,
        Sub = user.ExternalId,
        Email = user.Email,
        Name = user.Name,
        Role = user.Role.ToString(),
        IsOnboarded = user.IsOnboarded,
        Institution = user.Institution?.ToInstitutionDto(),
        StudyProgram = user.StudyProfile?.StudyProgram?.ToStudyProgramDto(),
        StudyProfile = user.StudyProfile?.ToStudyProfileDto()
    };

    public static AuthMeResponseDto ToUnauthenticatedDto() => new()
    {
        IsAuthenticated = false
    };
}
