using Domain.Enums;

namespace Application.Contracts.Features.DispatchDocuments.Queries.GetDispatchDocument;

public sealed class GetDispatchDocumentResponseDto
{
    public required string DocumentNumber { get; set; }
    public required StateType State { get; set; }
    public required int ClientId { get; set; }
    public required string ClientName { get; set; }
    public required DateOnly DateOnly { get; set; }
    public required List<DocumentResourceDto> ResourceDtos { get; init; }
    public class DocumentResourceDto
    {
        public required int Id { get; set; }
        public required int ResourceId { get; set; }
        public required string ResourceName { get; set; }
        public required int MeasureUnitId { get; set; }
        public required string MeasureUnitName { get; set; }
        public required int Count { get; set; }
    }
}