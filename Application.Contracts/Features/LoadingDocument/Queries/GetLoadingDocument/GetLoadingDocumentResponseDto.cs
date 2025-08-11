namespace Application.Contracts.Features.LoadingDocument.Queries.GetLoadingDocument;

public sealed class GetLoadingDocumentResponseDto
{
    public required string DocumentNumber { get; init; }
    public required DateOnly DateOnly { get; init; }
    public required List<LoadingDocumentResourceDto> ResourceDtos { get; init; }
    public class LoadingDocumentResourceDto
    {
        public required int Id { get; init; }
        public required int ResourceId { get; init; }
        public required string ResourceName { get; init; }
        public required int MeasureUnitId { get; init; }
        public required string MeasureUnitName { get; init; }
        public required int Count { get; init; }
    }
}