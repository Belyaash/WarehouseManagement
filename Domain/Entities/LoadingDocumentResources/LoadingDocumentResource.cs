using Domain.Entities.LoadingDocumentResources.Parameters;
using Domain.Entities.LoadingDocuments;
using Domain.Entities.MeasureUnits;
using Domain.Entities.Resources;

namespace Domain.Entities.LoadingDocumentResources;

public class LoadingDocumentResource
{
    private LoadingDocumentResource()
    {
    }

    public LoadingDocumentResource(CreateLoadingDocumentResourceParameters parameters)
    {
        DomainResource = parameters.DomainResource;
        LoadingDocument = parameters.LoadingDocument;
        MeasureUnit = parameters.MeasureUnit;
    }

    public int Id { get; private set; }
    public int Count { get; private set; }

    public int ResourceId { get; private set; }
    public DomainResource DomainResource { get; private set; } = default!;

    public int LoadingDocumentId { get; private set; }
    public LoadingDocument LoadingDocument { get; private set; } = default!;

    public int MeasureUnitId { get; private set; }
    public MeasureUnit MeasureUnit { get; private set; } = default!;
}