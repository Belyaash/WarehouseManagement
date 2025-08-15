namespace Application.Contracts.Features.LoadingDocument.Queries.GetLoadingDocument;

public sealed class GetLoadingDocumentResponseDto
{
    public required string DocumentNumber { get; set; }
    public required DateOnly DateOnly { get; set; }
    public required List<LoadingDocumentResourceDto> ResourceDtos { get; set; }
    public class LoadingDocumentResourceDto
    {
        public required int Id { get; set; }
        public required int ResourceId { get; set; }
        public required string ResourceName { get; set; }
        public required int MeasureUnitId { get; set; }
        public required string MeasureUnitName { get; set; }
        public required int Count { get; set; }
    }
}