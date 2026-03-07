using ExchangeMapper.Application.Interfaces.Repositories;
using ExchangeMapper.Domain.Entities;
using ExchangeMapper.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ExchangeMapper.Infrastructure.Persistence.Repositories;

public class UserRepository(AppDbContext context) : IUserRepository
{
    public async Task<User?> GetByExternalIdAsync(string externalId)
    {
        return await context.Users.FirstOrDefaultAsync(x => x.ExternalId == externalId);
    }

    public async Task<User?> GetByExternalIdWithDetailsAsync(string externalId)
    {
        return await context.Users
            .Include(u => u.UserInstitutions)
                .ThenInclude(ui => ui.Institution)
            .Include(u => u.UserInstitutions)
                .ThenInclude(ui => ui.StudyProfile)
                    .ThenInclude(sp => sp!.StudyProgram)
            .Include(u => u.UserInstitutions)
                .ThenInclude(ui => ui.Exchanges)
            .FirstOrDefaultAsync(u => u.ExternalId == externalId);
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await context.Users.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task AddAsync(User user)
    {
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(User user)
    {
        context.Users.Update(user);
        await context.SaveChangesAsync();
    }
}
