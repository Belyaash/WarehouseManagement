using Domain.Enums;

namespace Application.Contracts.Features.Client.Commands.UpdateClientState;

public sealed class UpdateClientStateRequestDto
{
    public required int Id { get; init; }
    public required StateType StateType { get; init; }
}