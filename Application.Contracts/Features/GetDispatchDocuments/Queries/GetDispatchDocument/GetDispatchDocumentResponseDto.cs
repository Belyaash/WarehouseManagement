using Domain.Enums;

namespace Application.Contracts.Features.GetDispatchDocuments.Queries.GetDispatchDocument;

public sealed class GetDispatchDocumentResponseDto
{
    public required string DocumentNumber { get; init; }
    public required StateType State { get; init; }
    public required int ClientId { get; init; }
    public required string ClientName { get; init; }
    public required DateOnly DateOnly { get; init; }
    public required List<DocumentResourceDto> ResourceDtos { get; init; }
    public class DocumentResourceDto
    {
        public required int Id { get; init; }
        public required int ResourceId { get; init; }
        public required string ResourceName { get; init; }
        public required int MeasureUnitId { get; init; }
        public required string MeasureUnitName { get; init; }
        public required int Count { get; init; }
    }
}