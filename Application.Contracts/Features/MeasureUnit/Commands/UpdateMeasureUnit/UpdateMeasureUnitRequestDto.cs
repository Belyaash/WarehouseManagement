namespace Application.Contracts.Features.MeasureUnit.Commands.UpdateMeasureUnit;

public sealed class UpdateMeasureUnitRequestDto
{
    public required int Id { get; init; }
    public required string Name { get; init; }
}