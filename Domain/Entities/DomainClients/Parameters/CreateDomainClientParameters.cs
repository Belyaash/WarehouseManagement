namespace Domain.Entities.DomainClients.Parameters;

public sealed class CreateDomainClientParameters
{
    public required string Name { get; init; }
    public required string Address { get; init; }
}