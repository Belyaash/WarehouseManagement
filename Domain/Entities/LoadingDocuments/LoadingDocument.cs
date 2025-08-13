using System.ComponentModel.DataAnnotations;
using Domain.Entities.LoadingDocumentResources;
using Domain.Entities.LoadingDocuments.Parameters;
using Domain.Entities.Resources;
using LinqSpecs;

namespace Domain.Entities.LoadingDocuments;

public class LoadingDocument
{
    private LoadingDocument()
    {
    }

    public LoadingDocument(CreateLoadingDocumentParameters parameters)
    {
        DocumentNumber = parameters.DocumentNumber;
        DateOnly = parameters.DateOnly;
    }

    public int Id { get; private set; }

    [MaxLength(255)]
    public string DocumentNumber { get; set; } = default!;
    public DateOnly DateOnly { get; set; }

    private List<LoadingDocumentResource> _resources = new();
    public IReadOnlyList<LoadingDocumentResource> Resources => _resources.AsReadOnly();

    #region Spec

    public static class Spec
    {
        public static Specification<LoadingDocument> ById(int id)
        {
            return new AdHocSpecification<LoadingDocument>(r => r.Id == id);
        }

        public static Specification<LoadingDocument> ByNumber(string documentNumber)
        {
            return new AdHocSpecification<LoadingDocument>(r => r.DocumentNumber == documentNumber);
        }

        public static Specification<LoadingDocument> FromDate(DateOnly dateOnly)
        {
            return new AdHocSpecification<LoadingDocument>(r => r.DateOnly >= dateOnly);
        }

        public static Specification<LoadingDocument> ToDate(DateOnly dateOnly)
        {
            return new AdHocSpecification<LoadingDocument>(r => r.DateOnly <= dateOnly);
        }

        public static Specification<LoadingDocument> IsNumberContains(List<string> documentNumbers)
        {
            return new AdHocSpecification<LoadingDocument>(r => documentNumbers.Contains(r.DocumentNumber));
        }

        public static Specification<LoadingDocument> IsResourceIdContains(List<int> resourceIds)
        {
            return new AdHocSpecification<LoadingDocument>(ld => ld.Resources.Any(r => resourceIds.Contains(r.DomainResourceId)));
        }

        public static Specification<LoadingDocument> IsMeasureUnitIdContains(List<int> measureUnitIds)
        {
            return new AdHocSpecification<LoadingDocument>(ld => ld.Resources.Any(r => measureUnitIds.Contains(r.MeasureUnitId)));
        }
    }

    #endregion
}