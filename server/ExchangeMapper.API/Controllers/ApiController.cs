using ErrorOr;
using ExchangeMapper.API.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ExchangeMapper.API.Controllers;

[ApiController]
public abstract class ApiController : ControllerBase
{
    protected IActionResult Match<T>(ErrorOr<T> result, Func<T, IActionResult> onSuccess)
    {
        return result.Match(onSuccess, errors => errors.ToProblemDetails(this));
    }

    protected Guid? GetCurrentUserId()
    {
        var rawUserId = User.FindFirstValue("userId");
        return Guid.TryParse(rawUserId, out var parsedUserId) ? parsedUserId : null;
    }
}
