using Domain.Enums;

namespace Application.Contracts.Features.MeasureUnit.Queries.GetMeasureUnit;

public sealed class GetMeasureUnitResponseDto
{
    public required string Name { get; set; }
    public required StateType State { get; init; }
    public required bool IsUsed { get; init; }
}