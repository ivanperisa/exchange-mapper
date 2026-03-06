using ExchangeMapper.Application.Interfaces;
using ExchangeMapper.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExchangeMapper.Infrastructure.Persistence.Repositories;

public class StudyProgramRepository(AppDbContext context) : IStudyProgramRepository
{
    public async Task<StudyProgram?> GetByIdAsync(Guid id)
    {
        return await context.StudyPrograms.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<StudyProgram>> GetByInstitutionIdAsync(Guid institutionId)
    {
        return await context.StudyPrograms
            .Where(x => x.InstitutionId == institutionId)
            .OrderBy(x => x.Name)
            .ToListAsync();
    }

    public async Task AddAsync(StudyProgram program)
    {
        await context.StudyPrograms.AddAsync(program);
        await context.SaveChangesAsync();
    }
}
