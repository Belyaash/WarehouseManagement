using Domain.Enums;

namespace Application.Contracts.Features.GetDispatchDocuments.Commands.UpdateDispatchDocument;

public sealed class UpdateDispatchDocumentRequestDto
{
    public required int Id { get; init; }
    public required string DocumentNumber { get; init; }
    public required int ClientId { get; init; }
    public required StateType State { get; init; }
    public required DateOnly DateOnly { get; init; }
    public required List<DocumentResourceDto> DocumentResources { get; init; }
    public class DocumentResourceDto
    {
        public required int ResourceId { get; init; }
        public required int MeasureUnitId { get; init; }
        public required int Count { get; init; }
    }
}