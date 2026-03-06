using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ExchangeMapper.Application.DTOs;

namespace ExchangeMapper.API.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class HomeController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        var sub = User.FindFirst("sub")?.Value ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var email = User.FindFirst("email")?.Value ?? User.FindFirst(ClaimTypes.Email)?.Value;

        return Ok(BaseResponse<object>.Ok(new
        {
            sub,
            email,
            datetime = DateTime.UtcNow,
            env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Unknown"
        }, GetRequestInfo()));
    }

    private RequestInfo GetRequestInfo()
    {
        return new RequestInfo
        {
            Method = HttpContext.Request.Method,
            Path = HttpContext.Request.Path,
            Timestamp = DateTime.UtcNow.ToString("O")
        };
    }
}
