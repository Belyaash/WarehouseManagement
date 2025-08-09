namespace Domain.Entities.LoadingDocuments.Parameters;

public sealed class CreateLoadingDocumentParameters
{
    public required string DocumentNumber { get; init; }
    public required DateOnly DateOnly { get; init; }
}