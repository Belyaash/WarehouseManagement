using System.ComponentModel.DataAnnotations;
using Domain.Entities.DispatchDocuments;
using Domain.Entities.DomainClients.Parameters;
using Domain.Enums;
using LinqSpecs;

namespace Domain.Entities.DomainClients;

public class DomainClient
{
    private DomainClient()
    {
    }

    public DomainClient(CreateDomainClientParameters parameters)
    {
        Name = parameters.Name;
        Address = parameters.Address;
        State = StateType.Actual;
    }

    public int Id { get; private set; }
    [MaxLength(255)]
    public string Name { get; private set; } = default!;
    public string Address { get; private set; } = default!;
    public StateType State { get; private set; }

    private List<DispatchDocument> _dispatchDocuments = new();
    public IReadOnlyList<DispatchDocument> DispatchDocuments => _dispatchDocuments.AsReadOnly();

    public void Update(UpdateDomainClientParameters parameters)
    {
        Name = parameters.Name;
        Address = parameters.Address;
    }

    public void ChangeState(ChangeDomainClientStateParameters parameters)
    {
        State = parameters.StateType;
    }

    #region Spec

    public static class Spec
    {
        public static Specification<DomainClient> ByName(string resourceName)
        {
            return new AdHocSpecification<DomainClient>(r => r.Name == resourceName);
        }

        public static Specification<DomainClient> ById(int id)
        {
            return new AdHocSpecification<DomainClient>(r => r.Id == id);
        }

        public static Specification<DomainClient> ByState(StateType type)
        {
            return new AdHocSpecification<DomainClient>(r => r.State == type);
        }

        public static Specification<DomainClient> CanBeDeleted()
        {
            return new AdHocSpecification<DomainClient>(r =>
                !r.DispatchDocuments.Any());
        }
    }
    #endregion
}