using ExchangeMapper.Domain.Enums;

namespace ExchangeMapper.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string ExternalId { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public bool IsOnboarded { get; set; }
    public DateTime CreatedAt { get; set; }

    public ICollection<UserInstitution> UserInstitutions { get; set; } = [];
    public ICollection<Exchange> StudentExchanges { get; set; } = [];
    public ICollection<MappingHistory> MappingHistoryChanges { get; set; } = [];
}
