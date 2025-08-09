using System.ComponentModel.DataAnnotations;
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
        DocumentNumber = parameters.DocumentNumber;
        Client = parameters.Client;
        DateOnly = parameters.DateOnly;
        State = StateType.Archived;
    }

    public int Id { get; private set; }

    [MaxLength(255)]
    public string DocumentNumber { get; private set; } = default!;

    public int ClientId { get; private set; }
    public DomainClient Client { get; private set; } = default!;

    public DateOnly DateOnly { get; set; }
    public StateType State { get; set; }

    private List<DispatchDocumentResource> _dispatchDocumentResources = new();
    public IReadOnlyList<DispatchDocumentResource> DispatchDocumentResources => _dispatchDocumentResources.AsReadOnly();
}