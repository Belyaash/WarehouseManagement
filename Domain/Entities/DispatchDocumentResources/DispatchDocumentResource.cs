using Domain.Entities.DispatchDocumentResources.Parameters;
using Domain.Entities.DispatchDocuments;
using Domain.Entities.MeasureUnits;
using Domain.Entities.Resources;

namespace Domain.Entities.DispatchDocumentResources;

public class DispatchDocumentResource
{
    private DispatchDocumentResource()
    {
    }

    public DispatchDocumentResource(CreateDispatchDocumentResourceParameters parameters)
    {
        DomainResource = parameters.DomainResource;
        MeasureUnit = parameters.MeasureUnit;
        DispatchDocument = parameters.DispatchDocument;
    }

    public int Id { get; private set; }
    public int ResourceId { get; private set; }
    public DomainResource DomainResource { get; private set; } = default!;

    public int MeasureUnitId { get; private set; }
    public MeasureUnit MeasureUnit { get; private set; } = default!;

    public int DispatchDocumentId { get; private set; }
    public DispatchDocument DispatchDocument { get; private set; } = default!;
}