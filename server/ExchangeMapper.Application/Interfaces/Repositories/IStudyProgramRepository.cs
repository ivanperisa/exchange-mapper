using ExchangeMapper.Domain.Entities;

namespace ExchangeMapper.Application.Interfaces.Repositories;

public interface IStudyProgramRepository
{
    Task<StudyProgram?> GetByIdAsync(Guid id);
    Task<List<StudyProgram>> GetByInstitutionIdAsync(Guid institutionId);
    Task AddAsync(StudyProgram program);
}
