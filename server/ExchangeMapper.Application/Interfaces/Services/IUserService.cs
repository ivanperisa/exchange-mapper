using ErrorOr;
using ExchangeMapper.Application.DTOs.Requests;
using ExchangeMapper.Domain.Entities;
using ExchangeMapper.Domain.Enums;

namespace ExchangeMapper.Application.Interfaces.Services;

public interface IUserService
{
    Task<ErrorOr<User>> SyncUserAsync(string externalId, string email, string name);
    Task<ErrorOr<User>> GetByExternalIdAsync(string externalId);
    Task<ErrorOr<User>> GetByExternalIdWithDetailsAsync(string externalId);
    Task<ErrorOr<Success>> CompleteOnboardingAsync(Guid userId, OnboardingRequestDto request);
    Task<ErrorOr<Success>> AddInstitutionAsync(Guid userId, InstitutionEntryDto request, UserRole role);
    Task<ErrorOr<Success>> UpdateInstitutionAsync(Guid userId, Guid userInstitutionId, InstitutionEntryDto request, UserRole role);
    Task<ErrorOr<Success>> RemoveInstitutionAsync(Guid userId, Guid userInstitutionId);
    Task<ErrorOr<Success>> MakeCoordinatorAsync(Guid userId);
}
