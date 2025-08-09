using Domain.Entities.DispatchDocumentResources;
using Domain.Entities.DispatchDocuments.Parameters;
using Domain.Entities.DomainClients;
using Domain.Enums;

namespace Domain.Entities.DispatchDocuments;

public class DispatchDocument
{
    private DispatchDocument()
    {
    }

    public DispatchDocument(CreateDispatchDocumentParameters parameters)
    {
        Number = parameters.Number;
        Client = parameters.Client;
        DateOnly = parameters.DateOnly;
        State = StateType.Archived;
    }

    public int Id { get; private set; }
    public int Number { get; private set; }

    public int ClientId { get; private set; }
    public DomainClient Client { get; private set; } = default!;

    public DateOnly DateOnly { get; set; }
    public StateType State { get; set; }

    private List<DispatchDocumentResource> _dispatchDocumentResources = new();
    public IReadOnlyList<DispatchDocumentResource> DispatchDocumentResources => _dispatchDocumentResources.AsReadOnly();
}