namespace Application.Contracts.Features.LoadingDocument.Commands.InsertLoadingDocument;

public sealed class InsertLoadingDocumentRequestDto
{
    public required string DocumentNumber { get; init; }
    public required DateOnly DateOnly { get; init; }
    public required List<DocumentResourceDto> DocumentResources { get; init; }
    public class DocumentResourceDto
    {
        public required int ResourceId { get; init; }
        public required int MeasureUnitId { get; init; }
        public required int Count { get; init; }
    }
}