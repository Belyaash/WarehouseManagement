using Domain.Entities.DomainClients;
using Domain.Enums;

namespace Domain.Entities.DispatchDocuments.Parameters;

public sealed class CreateDispatchDocumentParameters
{
    public required string DocumentNumber { get; init; }
    public required DomainClient Client { get; init; }
    public required DateOnly DateOnly { get; init; }
    public required StateType State { get; init; }
}