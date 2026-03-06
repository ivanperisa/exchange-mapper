using ExchangeMapper.Application.Interfaces;
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
                await userService.SyncUserAsync(sub, email, name ?? email);
            }
        }

        await next(context);
    }
}
