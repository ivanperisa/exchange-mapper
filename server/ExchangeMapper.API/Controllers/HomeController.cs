using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ExchangeMapper.API.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class HomeController : ApiController
{
    [HttpGet]
    public IActionResult Get()
    {
        var sub = User.FindFirst("sub")?.Value ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var email = User.FindFirst("email")?.Value ?? User.FindFirst(ClaimTypes.Email)?.Value;

        return Ok(new
        {
            sub,
            email,
            datetime = DateTime.UtcNow,
            env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Unknown"
        });
    }
}
