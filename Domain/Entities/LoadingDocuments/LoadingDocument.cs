using System.ComponentModel.DataAnnotations;
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

    [MaxLength(255)]
    public string DocumentNumber { get; set; } = default!;
    public DateOnly DateOnly { get; set; }

    private List<LoadingDocumentResource> _resources = new();
    public IReadOnlyList<LoadingDocumentResource> Resources => _resources.AsReadOnly();
}