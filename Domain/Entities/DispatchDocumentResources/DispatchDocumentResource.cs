using Domain.Entities.Balances;
using Domain.Entities.DispatchDocumentResources.Parameters;
using Domain.Entities.DispatchDocuments;
using Domain.Entities.MeasureUnits;
using Domain.Entities.Resources;
using Domain.Enums;

namespace Domain.Entities.DispatchDocumentResources;

public class DispatchDocumentResource
{
    private DispatchDocumentResource()
    {
    }

    public DispatchDocumentResource(CreateDispatchDocumentResourceParameters parameters)
    {
        DomainResource = parameters.DomainResource;
        MeasureUnit = parameters.MeasureUnit;
        DispatchDocument = parameters.DispatchDocument;
        Balance = parameters.Balance;
        Count = parameters.Count;

        if (parameters.DispatchDocument.State == StateType.Actual)
        {
            Balance.Count -= Count;
        }
    }

    public int Id { get; private set; }
    public int ResourceId { get; private set; }
    public DomainResource DomainResource { get; private set; } = default!;

    public int MeasureUnitId { get; private set; }
    public MeasureUnit MeasureUnit { get; private set; } = default!;

    public int DispatchDocumentId { get; private set; }
    public DispatchDocument DispatchDocument { get; private set; } = default!;

    public int BalanceId { get; private set; }
    public Balance Balance { get; private set; } = default!;

    public int Count { get; set; }
}