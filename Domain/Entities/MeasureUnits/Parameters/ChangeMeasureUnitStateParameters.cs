using Domain.Enums;

namespace Domain.Entities.MeasureUnits.Parameters;

public sealed class ChangeMeasureUnitStateParameters
{
    public required StateType StateType { get; init; }
}