using Domain.Enums;

namespace Application.Contracts.Features.MeasureUnit.Queries.GetMeasureUnits;

public sealed class GetMeasureUnitsRequestDto
{
    public required StateType State { get; init; }
    public int? Skip { get; init; }
    public int? Take { get; init; }
}