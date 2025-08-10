using Domain.Entities.Balances.Parameters;
using Domain.Entities.MeasureUnits;
using Domain.Entities.Resources;
using LinqSpecs;

namespace Domain.Entities.Balances;

public class Balance
{
    private Balance()
    {
    }

    public Balance(CreateBalanceParameters parameters)
    {
        MeasureUnit = parameters.MeasureUnit;
        DomainResource = parameters.DomainResource;
    }

    public int Id { get; private set; }

    public int MeasureUnitId { get; private set; }
    public MeasureUnit MeasureUnit { get; private set; } = default!;

    public int ResourceId { get; private set; }
    public DomainResource DomainResource { get; private set; } = default!;

    public int Count { get; set; }

    #region Spec

    public static class Spec
    {
        public static Specification<Balance> ByResourcesContains(List<int> resourceIds)
        {
            return new AdHocSpecification<Balance>(r => resourceIds.Contains(r.ResourceId));
        }

        public static Specification<Balance> ByMeasureUnitsContains(List<int> measureUnitIds)
        {
            return new AdHocSpecification<Balance>(r => measureUnitIds.Contains(r.MeasureUnitId));
        }
    }

    #endregion
}