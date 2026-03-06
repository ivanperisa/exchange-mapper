using ExchangeMapper.Domain.Entities;

namespace ExchangeMapper.Application.Interfaces.Repositories;

public interface IInstitutionRepository
{
    Task<List<Institution>> GetHomeInstitutionsAsync();
    Task<Institution?> GetByIdAsync(Guid id);
    Task AddAsync(Institution institution);
}
