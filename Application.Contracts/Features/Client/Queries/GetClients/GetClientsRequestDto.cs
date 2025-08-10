using Domain.Enums;

namespace Application.Contracts.Features.Client.Queries.GetClients;

public sealed class GetClientsRequestDto
{
    public required StateType State { get; init; }
    public int? Skip { get; init; }
    public int? Take { get; init; }
}