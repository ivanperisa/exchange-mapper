using ExchangeMapper.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeMapper.API.Controllers;

[ApiController]
[Route("institutions")]
public class InstitutionController(
    IInstitutionService institutionService) : ApiController
{
    [HttpGet]
    public async Task<IActionResult> GetHomeInstitutions()
    {
        var result = await institutionService.GetHomeInstitutionsAsync();
        return Match(result, Ok);
    }

    [HttpGet("{id:guid}/programs")]
    public async Task<IActionResult> GetProgramsByInstitution(Guid id)
    {
        var result = await institutionService.GetProgramsByInstitutionAsync(id);
        return Match(result, Ok);
    }

    [HttpGet("{id:guid}/programs/{programId:guid}/profiles")]
    public async Task<IActionResult> GetProfilesByProgram(Guid id, Guid programId)
    {
        var result = await institutionService.GetProfilesByProgramAsync(id, programId);
        return Match(result, Ok);
    }
}
