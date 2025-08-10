namespace Application.Contracts.Features.Resource.Commands.UpdateResource;

public sealed class UpdateResourceRequestDto
{
    public required int Id { get; init; }
    public required string Name { get; init; }
}