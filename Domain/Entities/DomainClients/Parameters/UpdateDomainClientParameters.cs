namespace Domain.Entities.DomainClients.Parameters;

public sealed class UpdateDomainClientParameters
{
    public required string Name { get; init; }
    public required string Address { get; init; }
}