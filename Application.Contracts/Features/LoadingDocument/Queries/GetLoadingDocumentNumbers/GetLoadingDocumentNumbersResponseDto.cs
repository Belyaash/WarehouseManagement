namespace Application.Contracts.Features.LoadingDocument.Queries.GetLoadingDocumentNumbers;

public sealed class GetLoadingDocumentNumbersResponseDto
{
    public required List<string> DocumentNumbers { get; set; }
}