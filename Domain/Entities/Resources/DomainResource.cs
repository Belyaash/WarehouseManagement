using System.ComponentModel.DataAnnotations;
using Domain.Entities.Balances;
using Domain.Entities.DispatchDocumentResources;
using Domain.Entities.LoadingDocumentResources;
using Domain.Entities.Resources.Parameters;
using Domain.Enums;
using LinqSpecs;

namespace Domain.Entities.Resources;

public class DomainResource
{
    private DomainResource()
    {
    }

    public DomainResource(CreateResourceParameters parameters)
    {
        Name = parameters.Name;
        State = StateType.Actual;
    }

    public int Id { get; private set; }

    [MaxLength(255)]
    public string Name { get; private set; } = default!;
    public StateType State { get; private set; }

    public List<LoadingDocumentResource> LoadingDocumentResources = new();

    public List<DispatchDocumentResource> DispatchDocumentResources = new();

    public List<Balance> Balances = new();

    public void Update(UpdateResourceParameters parameters)
    {
        Name = parameters.Name;
    }

    public void ChangeState(ChangeResourceStateParameters parameters)
    {
        State = parameters.StateType;
    }

    #region Spec

    public static class Spec
    {
        public static Specification<DomainResource> ByIdsList(IEnumerable<int> ids)
        {
            return new AdHocSpecification<DomainResource>(r => ids.Contains(r.Id));
        }
        public static Specification<DomainResource> ByName(string resourceName)
        {
            return new AdHocSpecification<DomainResource>(r => r.Name == resourceName);
        }

        public static Specification<DomainResource> ById(int id)
        {
            return new AdHocSpecification<DomainResource>(r => r.Id == id);
        }

        public static Specification<DomainResource> ByState(StateType type)
        {
            return new AdHocSpecification<DomainResource>(r => r.State == type);
        }

        public static Specification<DomainResource> CanBeDeleted()
        {
            return new AdHocSpecification<DomainResource>(r =>
                   !(r.DispatchDocumentResources.Any()
                     ||
                     r.LoadingDocumentResources.Any()));
        }
    }

    #endregion
}