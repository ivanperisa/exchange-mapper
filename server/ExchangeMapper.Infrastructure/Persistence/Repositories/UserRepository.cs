using ExchangeMapper.Application.Interfaces;
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
            .Include(u => u.Institution)
            .Include(u => u.StudyProfile)
                .ThenInclude(sp => sp.StudyProgram)
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
