using Domain.Entities.Balances.Parameters;
using Domain.Entities.DispatchDocumentResources;
using Domain.Entities.LoadingDocumentResources;
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

    public int DomainResourceId { get; private set; }
    public DomainResource DomainResource { get; private set; } = default!;

    private List<DispatchDocumentResource> _dispatchDocumentResources = new();
    public IReadOnlyList<DispatchDocumentResource> DispatchDocumentResources => _dispatchDocumentResources;

    private List<LoadingDocumentResource> _loadingDocumentResources = new();
    public IReadOnlyList<LoadingDocumentResource> LoadingDocumentResources => _loadingDocumentResources;
    public int Count { get; set; }

    #region Spec

    public static class Spec
    {
        public static Specification<Balance> ByResourcesContains(IEnumerable<int> resourceIds)
        {
            return new AdHocSpecification<Balance>(r => resourceIds.Contains(r.DomainResourceId));
        }

        public static Specification<Balance> ByMeasureUnitsContains(IEnumerable<int> measureUnitIds)
        {
            return new AdHocSpecification<Balance>(r => measureUnitIds.Contains(r.MeasureUnitId));
        }

        public static Specification<Balance> ByPositiveCount()
        {
            return new AdHocSpecification<Balance>(r => r.Count > 0);
        }    }

    #endregion
}