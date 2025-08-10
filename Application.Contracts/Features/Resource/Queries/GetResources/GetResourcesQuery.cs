using MediatR;

namespace Application.Contracts.Features.Resource.Queries.GetResources;

public sealed record GetResourcesQuery(GetResourcesRequestDto Dto) : IRequest<GetResourcesResponseDto>;