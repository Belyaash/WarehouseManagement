namespace Application.Contracts.Features.DispatchDocuments.Queries.GetDispatchDocumentNumbers;

public sealed class GetDispatchDocumentNumbersResponseDto
{
    public required List<string> DocumentNumbers { get; set; }
}