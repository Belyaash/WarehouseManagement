using Domain.Entities.LoadingDocumentResources;
using Domain.Entities.LoadingDocuments.Parameters;
using Domain.Entities.Resources;

namespace Domain.Entities.LoadingDocuments;

public class LoadingDocument
{
    private LoadingDocument()
    {
    }

    public LoadingDocument(CreateLoadingDocumentParameters parameters)
    {
        DocumentNumber = parameters.DocumentNumber;
        DateOnly = parameters.DateOnly;
    }

    public int Id { get; private set; }
    public int DocumentNumber { get; set; }
    public DateOnly DateOnly { get; set; }

    private List<LoadingDocumentResource> _resources = new();
    public IReadOnlyList<LoadingDocumentResource> Resources => _resources.AsReadOnly();
}