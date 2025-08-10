using Domain.Enums;

namespace Application.Contracts.Features.Resource.Commands.UpdateResourceState;

public sealed class UpdateResourceStateRequestDto
{
    public required int Id { get; init; }
    public required StateType StateType { get; init; }
}