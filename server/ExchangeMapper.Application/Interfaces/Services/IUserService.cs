using ErrorOr;
using ExchangeMapper.Application.DTOs.Requests;
using ExchangeMapper.Domain.Entities;

namespace ExchangeMapper.Application.Interfaces.Services;

public interface IUserService
{
    Task<ErrorOr<User>> SyncUserAsync(string externalId, string email, string name);
    Task<ErrorOr<User>> GetByExternalIdAsync(string externalId);
    Task<ErrorOr<User>> GetByExternalIdWithDetailsAsync(string externalId);
    Task<ErrorOr<Success>> CompleteOnboardingAsync(Guid userId, CompleteOnboardingRequestDto request);
    Task<ErrorOr<Success>> MakeCoordinatorAsync(Guid userId);
}
