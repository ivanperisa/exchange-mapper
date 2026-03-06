using System.Security.Claims;
using ExchangeMapper.API.Extensions;
using ExchangeMapper.Application.DTOs.Requests;
using ExchangeMapper.Application.Interfaces.Services;
using ExchangeMapper.Application.Mappers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeMapper.API.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController(
    IConfiguration configuration,
    IUserService userService) : ApiController
{
    [AllowAnonymous]
    [HttpGet("login")]
    public IActionResult Login([FromQuery] string? returnUrl = "/")
    {
        var clientId = configuration["Google:ClientId"];
        var clientSecret = configuration["Google:ClientSecret"];
        if (string.IsNullOrWhiteSpace(clientId) || string.IsNullOrWhiteSpace(clientSecret))
        {
            return Problem(
                detail: "Google OAuth is not configured. Set Google:ClientId and Google:ClientSecret.",
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Configuration Error",
                extensions: new Dictionary<string, object?> { ["code"] = "CONFIGURATION_ERROR" });
        }

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

    [AllowAnonymous]
    [HttpGet("me")]
    public async Task<IActionResult> Me()
    {
        var sub = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
               ?? User.FindFirst("sub")?.Value;

        if (sub is null)
        {
            return Ok(UserMapper.ToUnauthenticatedDto());
        }

        var user = await userService.GetByExternalIdWithDetailsAsync(sub);
        if (user.IsError)
        {
            return Ok(UserMapper.ToUnauthenticatedDto());
        }

        return Ok(user.Value.ToAuthMeResponseDto());
    }

    [AllowAnonymous]
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
    [HttpPost("onboarding")]
    public async Task<IActionResult> Onboarding([FromBody] CompleteOnboardingRequestDto request)
    {
        var userId = GetCurrentUserId();
        if (userId is null)
        {
            return Unauthorized();
        }

        var result = await userService.CompleteOnboardingAsync(userId.Value, request);
        return Match(result, _ => Ok());
    }

    [Authorize]
    [HttpPost("make-coordinator")]
    public async Task<IActionResult> MakeCoordinator([FromBody] MakeCoordinatorRoleRequestDto request)
    {
        var result = await userService.MakeCoordinatorAsync(request.UserId);
        return Match(result, _ => Ok());
    }

    [Authorize]
    [HttpGet("token")]
    public async Task<IActionResult> Token()
    {
        var accessToken = await HttpContext.GetTokenAsync("access_token");
        return Ok(new { accessToken });
    }
}
