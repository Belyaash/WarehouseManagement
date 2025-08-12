using System.ComponentModel.DataAnnotations;
using Domain.Entities.DispatchDocumentResources;
using Domain.Entities.DispatchDocuments.Parameters;
using Domain.Entities.DomainClients;
using Domain.Enums;
using LinqSpecs;

namespace Domain.Entities.DispatchDocuments;

public class DispatchDocument
{
    private DispatchDocument()
    {
    }

    public DispatchDocument(CreateDispatchDocumentParameters parameters)
    {
        DocumentNumber = parameters.DocumentNumber;
        Client = parameters.Client;
        DateOnly = parameters.DateOnly;
        State = parameters.State;
    }

    public int Id { get; private set; }

    [MaxLength(255)]
    public string DocumentNumber { get; set; } = default!;

    public int ClientId { get; set; }
    public DomainClient Client { get; set; } = default!;

    public DateOnly DateOnly { get; set; }
    public StateType State { get; set; }

    private List<DispatchDocumentResource> _dispatchDocumentResources = new();
    public IReadOnlyList<DispatchDocumentResource> DispatchDocumentResources => _dispatchDocumentResources.AsReadOnly();

    public void SetState(StateType state)
    {
        if (state == State) return;

        switch (state)
        {
            case StateType.Actual:
                State = StateType.Actual;
                _dispatchDocumentResources.ForEach(ddr => ddr.Balance.Count -= ddr.Count);
                break;
            case StateType.Archived:
                State = StateType.Archived;
                _dispatchDocumentResources.ForEach(ddr => ddr.Balance.Count += ddr.Count);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }

    #region Spec
    public static class Spec
    {
        public static Specification<DispatchDocument> ById(int id)
        {
            return new AdHocSpecification<DispatchDocument>(r => r.Id == id);
        }

        public static Specification<DispatchDocument> ByState(StateType stateType)
        {
            return new AdHocSpecification<DispatchDocument>(r => r.State == stateType);
        }

        public static Specification<DispatchDocument> ByNumber(string documentNumber)
        {
            return new AdHocSpecification<DispatchDocument>(r => r.DocumentNumber == documentNumber);
        }

        public static Specification<DispatchDocument> FromDate(DateOnly dateOnly)
        {
            return new AdHocSpecification<DispatchDocument>(r => r.DateOnly >= dateOnly);
        }

        public static Specification<DispatchDocument> ToDate(DateOnly dateOnly)
        {
            return new AdHocSpecification<DispatchDocument>(r => r.DateOnly <= dateOnly);
        }

        public static Specification<DispatchDocument> IsNumberContains(List<string> documentNumbers)
        {
            return new AdHocSpecification<DispatchDocument>(r => documentNumbers.Contains(r.DocumentNumber));
        }

        public static Specification<DispatchDocument> IsResourceIdContains(List<int> resourceIds)
        {
            return new AdHocSpecification<DispatchDocument>(ld => ld.DispatchDocumentResources.Any(r => resourceIds.Contains(r.ResourceId)));
        }

        public static Specification<DispatchDocument> IsMeasureUnitIdContains(List<int> measureUnitIds)
        {
            return new AdHocSpecification<DispatchDocument>(ld => ld.DispatchDocumentResources.Any(r => measureUnitIds.Contains(r.MeasureUnitId)));
        }

        public static Specification<DispatchDocument> IsClientIdContains(List<int> clientIds)
        {
            return new AdHocSpecification<DispatchDocument>(ld => clientIds.Contains(ld.ClientId));
        }
    }
    #endregion
}