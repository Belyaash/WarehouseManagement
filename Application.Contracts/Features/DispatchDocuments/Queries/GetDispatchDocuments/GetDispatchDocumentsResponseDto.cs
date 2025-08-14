using Domain.Enums;

namespace Application.Contracts.Features.DispatchDocuments.Queries.GetDispatchDocuments;

public sealed class GetDispatchDocumentsResponseDto
{
    public required List<DispatchDocumentDto> DispatchDocumentDtos { get; set; }

    public class DispatchDocumentDto
    {
        public required int Id { get; set; }
        public required string DocumentNumber { get; set; }
        public required DateOnly DateOnly { get; set; }
        public required int ClientId { get; set; }
        public required string ClientName { get; set; }
        public required StateType State { get; set; }
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