using ErrorOr;
using ExchangeMapper.Application.DTOs.Responses;

namespace ExchangeMapper.Application.Interfaces.Services;

public interface IInstitutionService
{
    Task<ErrorOr<List<InstitutionResponseDto>>> GetHomeInstitutionsAsync();
    Task<ErrorOr<List<StudyProgramResponseDto>>> GetProgramsByInstitutionAsync(Guid institutionId);
    Task<ErrorOr<List<StudyProfileResponseDto>>> GetProfilesByProgramAsync(Guid institutionId, Guid programId);
}
