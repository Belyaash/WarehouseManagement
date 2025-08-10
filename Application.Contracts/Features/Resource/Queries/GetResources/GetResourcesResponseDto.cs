namespace Application.Contracts.Features.Resource.Queries.GetResources;

public sealed class GetResourcesResponseDto
{
    public required List<GetResourceDto> Resources { get; init; }
    public class GetResourceDto
    {
        public required int Id { get; init; }
        public required string Name { get; init; }
    }
}