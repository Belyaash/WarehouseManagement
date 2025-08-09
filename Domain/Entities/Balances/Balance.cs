using Domain.Entities.Balances.Parameters;
using Domain.Entities.MeasureUnits;
using Domain.Entities.Resources;

namespace Domain.Entities.Balances;

public class Balance
{
    private Balance()
    {
    }

    public Balance(CreateBalanceParameters parameters)
    {
        MeasureUnit = parameters.MeasureUnit;
        Resource = parameters.Resource;
    }

    public int Id { get; private set; }

    public int MeasureUnitId { get; private set; }
    public MeasureUnit MeasureUnit { get; private set; } = default!;

    public int ResourceId { get; private set; }
    public Resource Resource { get; private set; } = default!;

    public int Count { get; set; }
}