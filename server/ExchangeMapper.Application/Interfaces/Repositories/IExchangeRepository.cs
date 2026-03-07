namespace ExchangeMapper.Application.Interfaces.Repositories;

public interface IExchangeRepository
{
    Task<bool> ExistsForUserInstitutionAsync(Guid userInstitutionId);
}
