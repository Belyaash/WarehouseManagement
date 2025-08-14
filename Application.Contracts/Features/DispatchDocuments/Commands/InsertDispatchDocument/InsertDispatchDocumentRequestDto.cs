using Domain.Enums;

namespace Application.Contracts.Features.DispatchDocuments.Commands.InsertDispatchDocument;

public sealed class InsertDispatchDocumentRequestDto
{
    public required string DocumentNumber { get; init; }
    public required int ClientId { get; init; }
    public required StateType StateType { get; init; }
    public required DateOnly DateOnly { get; init; }
    public required List<DocumentResourceDto> DocumentResources { get; init; }
    public class DocumentResourceDto
    {
        public required int ResourceId { get; init; }
        public required int MeasureUnitId { get; init; }
        public required int Count { get; init; }
    }
}