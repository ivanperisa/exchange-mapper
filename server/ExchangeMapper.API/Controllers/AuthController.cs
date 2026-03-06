using System.Security.Claims;
using ExchangeMapper.API.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeMapper.API.Controllers;

[ApiController]
[Route("auth")]
public class AuthController(IConfiguration configuration) : ControllerBase
{
    [HttpGet("login")]
    public IActionResult Login([FromQuery] string? returnUrl = "/")
    {
        var frontendTarget = configuration.BuildFrontendUrl(returnUrl);
        if (User.Identity?.IsAuthenticated == true)
        {
            return Redirect(frontendTarget);
        }

        var authProperties = new AuthenticationProperties
        {
            RedirectUri = frontendTarget
        };

        return Challenge(authProperties, "GoogleOidc");
    }

    [HttpGet("me")]
    public IActionResult Me()
    {
        if (User.Identity?.IsAuthenticated != true)
        {
            return Ok(new { isAuthenticated = false });
        }

        return Ok(new
        {
            isAuthenticated = true,
            sub = User.FindFirstNonEmptyClaim("sub", ClaimTypes.NameIdentifier),
            email = User.FindFirstNonEmptyClaim("email", ClaimTypes.Email),
            name = User.FindFirstNonEmptyClaim("name", ClaimTypes.Name)
        });
    }

    [HttpGet("logout")]
    public IActionResult LogoutRedirect()
    {
        return SignOut(
            new AuthenticationProperties { RedirectUri = configuration.BuildFrontendUrl("/") },
            CookieAuthenticationDefaults.AuthenticationScheme);
    }

    [Authorize]
    [HttpPost("logout")]
    public IActionResult Logout()
    {
        var authProperties = new AuthenticationProperties
        {
            RedirectUri = configuration.BuildFrontendUrl("/")
        };

        return SignOut(authProperties, CookieAuthenticationDefaults.AuthenticationScheme);
    }

    [Authorize]
    [HttpGet("token")]
    public async Task<IActionResult> Token()
    {
        var accessToken = await HttpContext.GetTokenAsync("access_token");
        return Ok(new { accessToken });
    }
}
