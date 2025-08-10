using Domain.Enums;

namespace Application.Contracts.Features.Client.Queries.GetClient;

public sealed class GetClientResponseDto
{
    public required string Name { get; init; }
    public required StateType State { get; init; }
    public required string Address { get; init; }
    public required bool IsUsed { get; init; }
}