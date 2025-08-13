using Domain.Enums;

namespace Application.Contracts.Features.Client.Queries.GetClient;

public sealed class GetClientResponseDto
{
    public required string Name { get; set; }
    public required StateType State { get; init; }
    public required string Address { get; set; }
    public required bool IsUsed { get; init; }
}