using Domain.Entities.Balances;
using Domain.Entities.LoadingDocumentResources.Parameters;
using Domain.Entities.LoadingDocuments;
using Domain.Entities.MeasureUnits;
using Domain.Entities.Resources;
using LinqSpecs;

namespace Domain.Entities.LoadingDocumentResources;

public class LoadingDocumentResource
{
    private LoadingDocumentResource()
    {
    }

    public LoadingDocumentResource(CreateLoadingDocumentResourceParameters parameters)
    {
        DomainResource = parameters.DomainResource;
        LoadingDocument = parameters.LoadingDocument;
        MeasureUnit = parameters.MeasureUnit;
        Balance = parameters.Balance;
        if (Count <= 0)
            throw new ArgumentException("Количество должно быть больше 0.");
        Count = parameters.Count;
        Balance.Count += parameters.Count;
    }

    public int Id { get; private set; }
    public int Count { get; set; }

    public int DomainResourceId { get; private set; }
    public DomainResource DomainResource { get; private set; } = default!;

    public int LoadingDocumentId { get; private set; }
    public LoadingDocument LoadingDocument { get; private set; } = default!;

    public int MeasureUnitId { get; private set; }
    public MeasureUnit MeasureUnit { get; private set; } = default!;

    public int BalanceId { get; private set; }
    public Balance Balance { get; private set; } = default!;

    #region Spec

    public static class Spec
    {
        public static Specification<LoadingDocumentResource> ById(int id)
        {
            return new AdHocSpecification<LoadingDocumentResource>(r => r.Id == id);
        }

        public static Specification<LoadingDocumentResource> ByResourceId(int id)
        {
            return new AdHocSpecification<LoadingDocumentResource>(r => r.Id == id);
        }

        public static Specification<LoadingDocumentResource> ByMeasureUnitId(int id)
        {
            return new AdHocSpecification<LoadingDocumentResource>(r => r.Id == id);
        }

        public static Specification<LoadingDocumentResource> ByDocumentId(int id)
        {
            return new AdHocSpecification<LoadingDocumentResource>(r => r.LoadingDocumentId == id);
        }    }

    #endregion
}