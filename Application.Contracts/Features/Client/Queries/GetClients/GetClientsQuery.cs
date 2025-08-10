using MediatR;

namespace Application.Contracts.Features.Client.Queries.GetClients;

public sealed record GetClientsQuery(GetClientsRequestDto Dto) : IRequest<GetClientsResponseDto>;