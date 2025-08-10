using Domain.Entities.LoadingDocuments;
using Domain.Entities.MeasureUnits;
using Domain.Entities.Resources;

namespace Domain.Entities.LoadingDocumentResources.Parameters;

public sealed class CreateLoadingDocumentResourceParameters
{
    public required DomainResource DomainResource { get; init; }
    public required LoadingDocument LoadingDocument { get; init; }
    public required MeasureUnit MeasureUnit { get; init; }
}