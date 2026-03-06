namespace ExchangeMapper.API.Middleware;

public class UserSyncMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        if (context.User.Identity?.IsAuthenticated == true)
        {
            var sub = context.User.FindFirst("sub")?.Value;
            var email = context.User.FindFirst("email")?.Value;
            var name = context.User.FindFirst("name")?.Value;

            if (sub is not null && email is not null)
            {
                // TODO: inject IUserService i pozvati SyncUserAsync
                // await userService.SyncUserAsync(sub, email, name ?? email);
            }
        }

        await next(context);
    }
}