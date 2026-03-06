using System.Security.Claims;

namespace ExchangeMapper.API.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string? FindFirstNonEmptyClaim(this ClaimsPrincipal user, params string[] types)
    {
        foreach (var type in types)
        {
            var value = user.FindFirst(type)?.Value;
            if (!string.IsNullOrWhiteSpace(value))
            {
                return value;
            }
        }

        return null;
    }
}
