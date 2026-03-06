using ExchangeMapper.Application.DTOs;
using ExchangeMapper.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeMapper.API.Controllers;

[ApiController]
[Route("institutions")]
public class InstitutionController(
    IInstitutionRepository institutionRepository,
    IStudyProgramRepository studyProgramRepository,
    IStudyProfileRepository studyProfileRepository) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetHomeInstitutions()
    {
        var institutions = await institutionRepository.GetHomeInstitutionsAsync();
        var data = institutions.Select(institution => new InstitutionDto
        {
            Id = institution.Id,
            Name = institution.Name,
            Country = institution.Country,
            City = institution.City,
            ErasmusCode = institution.ErasmusCode
        }).ToList();

        return Ok(BaseResponse<List<InstitutionDto>>.Ok(data, GetRequestInfo()));
    }

    [HttpGet("{id:guid}/programs")]
    public async Task<IActionResult> GetProgramsByInstitution(Guid id)
    {
        var programs = await studyProgramRepository.GetByInstitutionIdAsync(id);
        var data = programs.Select(program => new StudyProgramDto
        {
            Id = program.Id,
            Name = program.Name,
            IscedCode = program.IscedCode
        }).ToList();

        return Ok(BaseResponse<List<StudyProgramDto>>.Ok(data, GetRequestInfo()));
    }

    [HttpGet("{id:guid}/programs/{programId:guid}/profiles")]
    public async Task<IActionResult> GetProfilesByProgram(Guid id, Guid programId)
    {
        var profiles = await studyProfileRepository.GetByProgramIdAsync(programId);
        var data = profiles.Select(profile => new StudyProfileDto
        {
            Id = profile.Id,
            Name = profile.Name
        }).ToList();

        return Ok(BaseResponse<List<StudyProfileDto>>.Ok(data, GetRequestInfo()));
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
