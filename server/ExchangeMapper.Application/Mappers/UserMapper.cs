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
        Institutions = user.UserInstitutions
            .OrderBy(ui => ui.CreatedAt)
            .Select(ui => ui.ToUserInstitutionDto())
            .ToList()
    };

    public static AuthMeResponseDto ToUnauthenticatedDto() => new()
    {
        IsAuthenticated = false
    };
}
