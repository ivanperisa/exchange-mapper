using ExchangeMapper.Application.Interfaces.Repositories;
using ExchangeMapper.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExchangeMapper.Infrastructure.Persistence.Repositories;

public class StudyProfileRepository(AppDbContext context) : IStudyProfileRepository
{
    public async Task<StudyProfile?> GetByIdAsync(Guid id)
    {
        return await context.StudyProfiles.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<StudyProfile>> GetByProgramIdAsync(Guid programId)
    {
        return await context.StudyProfiles
            .Where(x => x.StudyProgramId == programId)
            .OrderBy(x => x.Name)
            .ToListAsync();
    }

    public async Task AddAsync(StudyProfile profile)
    {
        await context.StudyProfiles.AddAsync(profile);
        await context.SaveChangesAsync();
    }
}
