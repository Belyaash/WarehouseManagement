using MediatR;

namespace Application.Contracts.Features.Resource.Queries.GetResource;

public sealed record GetResourceQuery(GetResourceRequestDto Dto) : IRequest<GetResourceResponseDto>;