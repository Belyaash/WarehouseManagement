using Domain.Enums;

namespace Application.Contracts.Features.Resource.Queries.GetResources;

public sealed class GetResourcesRequestDto
{
    public required StateType State { get; init; }
    public int? Skip { get; init; }
    public int? Take { get; init; }
}