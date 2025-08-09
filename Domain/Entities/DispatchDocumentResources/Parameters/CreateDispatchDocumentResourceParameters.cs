using Domain.Entities.DispatchDocuments;
using Domain.Entities.MeasureUnits;
using Domain.Entities.Resources;

namespace Domain.Entities.DispatchDocumentResources.Parameters;

public sealed class CreateDispatchDocumentResourceParameters
{
    public required Resource Resource { get; init; }
    public required MeasureUnit MeasureUnit { get; init; }
    public required DispatchDocument DispatchDocument { get; init; }
}