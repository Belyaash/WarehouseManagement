namespace Domain.Entities.LoadingDocuments.Parameters;

public sealed class CreateLoadingDocumentParameters
{
    public required int DocumentNumber { get; init; }
    public required DateOnly DateOnly { get; init; }
}