using MediatR;

namespace Application.Contracts.Features.Client.Queries.GetClient;

public sealed record GetClientQuery(GetClientRequestDto Dto) : IRequest<GetClientResponseDto>;