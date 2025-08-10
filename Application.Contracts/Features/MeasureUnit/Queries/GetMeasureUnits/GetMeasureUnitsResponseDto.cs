namespace Application.Contracts.Features.MeasureUnit.Queries.GetMeasureUnits;

public sealed class GetMeasureUnitsResponseDto
{
    public required List<GetMeasureUnitDto> MeasureUnits { get; init; }
    public class GetMeasureUnitDto
    {
        public required int Id { get; init; }
        public required string Name { get; init; }
    }
}