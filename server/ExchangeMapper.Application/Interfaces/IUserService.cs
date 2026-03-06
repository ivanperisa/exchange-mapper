using ExchangeMapper.Application.DTOs;
using ExchangeMapper.Domain.Entities;

namespace ExchangeMapper.Application.Interfaces;

public interface IUserService
{
    Task<User> SyncUserAsync(string externalId, string email, string name);
    Task<User?> GetByExternalIdAsync(string externalId);
    Task<User?> GetByExternalIdWithDetailsAsync(string externalId);
    Task CompleteOnboardingAsync(Guid userId, OnboardingRequestDto request);
    Task MakeCoordinatorAsync(Guid userId);
}
