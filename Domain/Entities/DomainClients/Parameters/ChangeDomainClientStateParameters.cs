using Domain.Enums;

namespace Domain.Entities.DomainClients.Parameters;

public sealed class ChangeDomainClientStateParameters
{
    public required StateType StateType { get; init; }
}