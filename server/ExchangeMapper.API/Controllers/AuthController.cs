using System.Security.Claims;
using ExchangeMapper.API.Extensions;
using ExchangeMapper.Application.DTOs;
using ExchangeMapper.Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeMapper.API.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController(
    IConfiguration configuration,
    IUserService userService,
    IValidator<OnboardingRequestDto> validator) : ControllerBase
{
    [AllowAnonymous]
    [HttpGet("login")]
    public IActionResult Login([FromQuery] string? returnUrl = "/")
    {
        var clientId = configuration["Google:ClientId"];
        var clientSecret = configuration["Google:ClientSecret"];
        if (string.IsNullOrWhiteSpace(clientId) || string.IsNullOrWhiteSpace(clientSecret))
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                BaseResponse<object>.Fail(
                    "CONFIGURATION_ERROR",
                    "Google OAuth is not configured. Set Google:ClientId and Google:ClientSecret.",
                    GetRequestInfo()));
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
        if (User.Identity?.IsAuthenticated != true)
        {
            var unauthenticatedResponse = new AuthMeResponseDto
            {
                IsAuthenticated = false,
                IsOnboarded = false
            };
            return Ok(BaseResponse<AuthMeResponseDto>.Ok(unauthenticatedResponse, GetRequestInfo()));
        }

        var sub = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;
        if (string.IsNullOrWhiteSpace(sub))
        {
            var noSubResponse = new AuthMeResponseDto
            {
                IsAuthenticated = false,
                IsOnboarded = false
            };
            return Ok(BaseResponse<AuthMeResponseDto>.Ok(noSubResponse, GetRequestInfo()));
        }

        var user = await userService.GetByExternalIdWithDetailsAsync(sub);
        if (user is null)
        {
            var missingUserResponse = new AuthMeResponseDto
            {
                IsAuthenticated = false,
                IsOnboarded = false
            };
            return Ok(BaseResponse<AuthMeResponseDto>.Ok(missingUserResponse, GetRequestInfo()));
        }

        var response = new AuthMeResponseDto
        {
            IsAuthenticated = true,
            Sub = sub,
            Email = User.FindFirst(ClaimTypes.Email)?.Value ?? User.FindFirst("email")?.Value ?? user.Email,
            Name = User.FindFirst(ClaimTypes.Name)?.Value ?? User.FindFirst("name")?.Value ?? user.Name,
            Role = user.Role.ToString(),
            IsOnboarded = user.IsOnboarded,
            Institution = user.Institution is null ? null : new InstitutionDto
            {
                Id = user.Institution.Id,
                Name = user.Institution.Name,
                Country = user.Institution.Country,
                City = user.Institution.City,
                ErasmusCode = user.Institution.ErasmusCode
            },
            StudyProfile = user.StudyProfile is null ? null : new StudyProfileDto
            {
                Id = user.StudyProfile.Id,
                Name = user.StudyProfile.Name
            },
            StudyProgram = user.StudyProfile?.StudyProgram is null ? null : new StudyProgramDto
            {
                Id = user.StudyProfile.StudyProgram.Id,
                Name = user.StudyProfile.StudyProgram.Name,
                IscedCode = user.StudyProfile.StudyProgram.IscedCode
            }
        };

        return Ok(BaseResponse<AuthMeResponseDto>.Ok(response, GetRequestInfo()));
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
    public async Task<IActionResult> CompleteOnboarding([FromBody] OnboardingRequestDto request)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return BadRequest(BaseResponse<object>.Fail(
                "VALIDATION_ERROR",
                string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)),
                GetRequestInfo()));
        }

        var externalId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;
        if (string.IsNullOrWhiteSpace(externalId))
        {
            return Unauthorized(BaseResponse<object>.Fail("UNAUTHORIZED", "Missing sub claim.", GetRequestInfo()));
        }

        var user = await userService.GetByExternalIdAsync(externalId);
        if (user is null)
        {
            return NotFound(BaseResponse<object>.Fail("NOT_FOUND", "User not found.", GetRequestInfo()));
        }

        await userService.CompleteOnboardingAsync(user.Id, request);
        return Ok(BaseResponse<object>.Ok(null, GetRequestInfo()));
    }

    [Authorize]
    [HttpPost("make-coordinator")]
    public async Task<IActionResult> MakeCoordinator([FromBody] MakeCoordinatorRequestDto request)
    {
        await userService.MakeCoordinatorAsync(request.UserId);
        return Ok(BaseResponse<object>.Ok(null, GetRequestInfo()));
    }

    [Authorize]
    [HttpGet("token")]
    public async Task<IActionResult> Token()
    {
        var accessToken = await HttpContext.GetTokenAsync("access_token");
        return Ok(BaseResponse<object>.Ok(new { accessToken }, GetRequestInfo()));
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
