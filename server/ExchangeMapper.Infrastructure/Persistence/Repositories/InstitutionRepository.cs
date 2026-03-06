using ExchangeMapper.Application.Interfaces;
using ExchangeMapper.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExchangeMapper.Infrastructure.Persistence.Repositories;

public class InstitutionRepository(AppDbContext context) : IInstitutionRepository
{
    public async Task<List<Institution>> GetHomeInstitutionsAsync()
    {
        return await context.Institutions
            .Where(x => x.IsHome)
            .OrderBy(x => x.Name)
            .ToListAsync();
    }

    public async Task<Institution?> GetByIdAsync(Guid id)
    {
        return await context.Institutions.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task AddAsync(Institution institution)
    {
        await context.Institutions.AddAsync(institution);
        await context.SaveChangesAsync();
    }
}
