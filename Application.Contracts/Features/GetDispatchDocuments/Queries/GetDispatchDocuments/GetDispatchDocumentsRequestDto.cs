namespace Application.Contracts.Features.GetDispatchDocuments.Queries.GetDispatchDocuments;

public sealed class GetDispatchDocumentsRequestDto
{
    public DateOnly? DateFrom { get; init; }
    public DateOnly? DateTo { get; init; }
    public List<string>? DocumentNumbersFilter { get; init; }
    public List<int>? ResourceFilter { get; init; }
    public List<int>? MeasureUnitFilter { get; init; }
    public List<int>? ClientFilter { get; init; }
    public int? Skip { get; init; }
    public int? Take { get; init; }
}