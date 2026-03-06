using ExchangeMapper.Domain.Entities;

namespace ExchangeMapper.Application.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User?> GetByExternalIdAsync(string externalId);
    Task<User?> GetByExternalIdWithDetailsAsync(string externalId);
    Task<User?> GetByIdAsync(Guid id);
    Task AddAsync(User user);
    Task UpdateAsync(User user);
}
