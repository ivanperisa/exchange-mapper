using ExchangeMapper.Application.Interfaces.Services;
using System.Security.Claims;

namespace ExchangeMapper.API.Middleware;

public class UserSyncMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, IUserService userService)
    {
        if (context.User.Identity?.IsAuthenticated == true)
        {
            var sub = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? context.User.FindFirst("sub")?.Value;
            var email = context.User.FindFirst(ClaimTypes.Email)?.Value
                ?? context.User.FindFirst("email")?.Value;
            var name = context.User.FindFirst(ClaimTypes.Name)?.Value
                ?? context.User.FindFirst("name")?.Value;

            if (!string.IsNullOrWhiteSpace(sub) && !string.IsNullOrWhiteSpace(email))
            {
                var syncResult = await userService.SyncUserAsync(sub, email, name ?? email);
                if (!syncResult.IsError && context.User.Identity is ClaimsIdentity identity)
                {
                    var user = syncResult.Value;
                    if (!identity.HasClaim(claim => claim.Type == "userId"))
                    {
                        identity.AddClaim(new Claim("userId", user.Id.ToString()));
                    }

                    if (!identity.HasClaim(claim => claim.Type == ClaimTypes.Role))
                    {
                        identity.AddClaim(new Claim(ClaimTypes.Role, user.Role.ToString()));
                    }
                }
            }
        }

        await next(context);
    }
}
