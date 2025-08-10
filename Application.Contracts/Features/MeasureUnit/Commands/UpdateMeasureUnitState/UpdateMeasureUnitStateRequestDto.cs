using Domain.Enums;

namespace Application.Contracts.Features.MeasureUnit.Commands.UpdateMeasureUnitState;

public sealed class UpdateMeasureUnitStateRequestDto
{
    public required int Id { get; init; }
    public required StateType StateType { get; init; }
}