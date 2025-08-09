using Domain.Entities.MeasureUnits;
using Domain.Entities.Resources;

namespace Domain.Entities.Balances.Parameters;

public sealed class CreateBalanceParameters
{
    public required MeasureUnit MeasureUnit { get; init; }
    public required Resource Resource { get; init; }
}