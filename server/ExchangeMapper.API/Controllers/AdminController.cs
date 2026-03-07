using ExchangeMapper.Application.DTOs.Requests;
using ExchangeMapper.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeMapper.API.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(Roles = "Coordinator")]
public class AdminController(IUserService userService) : ApiController
{
    [HttpPost("make-coordinator")]
    public async Task<IActionResult> MakeCoordinator([FromBody] MakeCoordinatorRoleRequestDto request)
    {
        var result = await userService.MakeCoordinatorAsync(request.UserId);
        return Match(result, _ => Ok());
    }
}
