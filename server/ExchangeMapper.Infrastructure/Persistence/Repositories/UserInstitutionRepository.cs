using ExchangeMapper.Application.Interfaces.Repositories;
using ExchangeMapper.Domain.Entities;
using ExchangeMapper.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ExchangeMapper.Infrastructure.Persistence.Repositories;

public class UserInstitutionRepository(AppDbContext context) : IUserInstitutionRepository
{
    public async Task<List<UserInstitution>> GetByUserIdAsync(Guid userId)
    {
        return await context.UserInstitutions
            .Include(ui => ui.Institution)
            .Include(ui => ui.StudyProfile)
                .ThenInclude(sp => sp!.StudyProgram)
            .Include(ui => ui.Exchanges)
            .Where(ui => ui.UserId == userId)
            .OrderBy(ui => ui.CreatedAt)
            .ToListAsync();
    }

    public async Task<UserInstitution?> GetByIdAsync(Guid id)
    {
        return await context.UserInstitutions
            .FirstOrDefaultAsync(ui => ui.Id == id);
    }

    public async Task AddAsync(UserInstitution userInstitution)
    {
        await context.UserInstitutions.AddAsync(userInstitution);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(UserInstitution userInstitution)
    {
        context.UserInstitutions.Update(userInstitution);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(UserInstitution userInstitution)
    {
        context.UserInstitutions.Remove(userInstitution);
        await context.SaveChangesAsync();
    }
}
