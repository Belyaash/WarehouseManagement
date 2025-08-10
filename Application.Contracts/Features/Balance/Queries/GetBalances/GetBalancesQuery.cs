using MediatR;

namespace Application.Contracts.Features.Balance.Queries.GetBalances;

public sealed record GetBalancesQuery(GetBalancesRequestDto Dto) : IRequest<GetBalancesResponseDto>;