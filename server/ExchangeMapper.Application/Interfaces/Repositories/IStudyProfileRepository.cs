using ExchangeMapper.Domain.Entities;

namespace ExchangeMapper.Application.Interfaces.Repositories;

public interface IStudyProfileRepository
{
    Task<StudyProfile?> GetByIdAsync(Guid id);
    Task<List<StudyProfile>> GetByProgramIdAsync(Guid programId);
    Task AddAsync(StudyProfile profile);
}
