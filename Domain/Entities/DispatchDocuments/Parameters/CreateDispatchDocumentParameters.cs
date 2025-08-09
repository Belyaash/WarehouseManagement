using Domain.Entities.DomainClients;

namespace Domain.Entities.DispatchDocuments.Parameters;

public sealed class CreateDispatchDocumentParameters
{
    public required string DocumentNumber { get; init; }
    public required DomainClient Client { get; init; }
    public required DateOnly DateOnly { get; init; }
}