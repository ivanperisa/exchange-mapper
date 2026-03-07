using ExchangeMapper.Domain.Entities;

namespace ExchangeMapper.Application.Interfaces.Repositories;

public interface IUserInstitutionRepository
{
    Task<List<UserInstitution>> GetByUserIdAsync(Guid userId);
    Task<UserInstitution?> GetByIdAsync(Guid id);
    Task AddAsync(UserInstitution userInstitution);
    Task UpdateAsync(UserInstitution userInstitution);
    Task DeleteAsync(UserInstitution userInstitution);
}
