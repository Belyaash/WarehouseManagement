using System.ComponentModel.DataAnnotations;
using Domain.Entities.Balances;
using Domain.Entities.DispatchDocumentResources;
using Domain.Entities.LoadingDocumentResources;
using Domain.Entities.LoadingDocuments;
using Domain.Entities.MeasureUnits.Parameters;
using Domain.Enums;
using LinqSpecs;

namespace Domain.Entities.MeasureUnits;

public class MeasureUnit
{
    private MeasureUnit()
    {
    }

    public MeasureUnit(CreateMeasureUnitParameters parameters)
    {
        Name = parameters.Name;
        State = StateType.Actual;
    }

    public int Id { get; private set; }

    [MaxLength(255)]
    public string Name { get; private set; } = default!;
    public StateType State { get; private set; }

    private List<LoadingDocumentResource> _loadingDocumentResource = new();
    public IReadOnlyList<LoadingDocumentResource> LoadingDocumentResources => _loadingDocumentResource;

    private List<DispatchDocumentResource> _dispatchDocumentResources = new();
    public IReadOnlyList<DispatchDocumentResource> DispatchDocumentResources => _dispatchDocumentResources;

    private List<Balance> _balances = new();
    public IReadOnlyList<Balance> Balances => _balances;

    public void Update(UpdateMeasureUnitParameters parameters)
    {
        Name = parameters.Name;
    }

    public void ChangeState(ChangeMeasureUnitStateParameters parameters)
    {
        State = parameters.StateType;
    }

    #region Spec

    public static class Spec
    {
        public static Specification<MeasureUnit> ByName(string resourceName)
        {
            return new AdHocSpecification<MeasureUnit>(r => r.Name == resourceName);
        }

        public static Specification<MeasureUnit> ByIdsList(IEnumerable<int> ids)
        {
            return new AdHocSpecification<MeasureUnit>(r => ids.Contains(r.Id));
        }

        public static Specification<MeasureUnit> ById(int id)
        {
            return new AdHocSpecification<MeasureUnit>(r => r.Id == id);
        }

        public static Specification<MeasureUnit> ByState(StateType type)
        {
            return new AdHocSpecification<MeasureUnit>(r => r.State == type);
        }

        public static Specification<MeasureUnit> CanBeDeleted()
        {
            return new AdHocSpecification<MeasureUnit>(r =>
                !(r.Balances.Any()
                  ||
                  r.DispatchDocumentResources.Any()
                  ||
                  r.LoadingDocumentResources.Any()));
        }
    }

    #endregion
}