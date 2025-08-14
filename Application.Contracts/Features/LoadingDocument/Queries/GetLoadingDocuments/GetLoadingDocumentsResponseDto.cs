namespace Application.Contracts.Features.LoadingDocument.Queries.GetLoadingDocuments;

public sealed class GetLoadingDocumentsResponseDto
{
    public required List<LoadingDocumentDto> LoadingDocuments { get; set; }

    public class LoadingDocumentDto
    {
        public required int Id { get; set; }
        public required string DocumentNumber { get; set; }
        public required DateOnly DateOnly { get; set; }
        public required List<DocumentResourceDto> DocumentResources { get; set; }
    }

    public class DocumentResourceDto
    {
        public required int ResourceId { get; set; }
        public required string ResourceName { get; set; }
        public required int MeasureUnitId { get; set; }
        public required string MeasureUnitName { get; set; }
        public required int Count { get; set; }
    }
}