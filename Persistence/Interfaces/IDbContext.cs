using Domain.Entities.Balances;
using Domain.Entities.DispatchDocumentResources;
using Domain.Entities.DispatchDocuments;
using Domain.Entities.DomainClients;
using Domain.Entities.LoadingDocumentResources;
using Domain.Entities.LoadingDocuments;
using Domain.Entities.MeasureUnits;
using Domain.Entities.Resources;

namespace Persistence.Interfaces;

public interface IDbContext
{
    IDbSet<Balance> Balances { get; }
    IDbSet<DispatchDocumentResource> DispatchDocumentResources { get; }
    IDbSet<DispatchDocument> DispatchDocuments { get; }
    IDbSet<DomainClient> DomainClients { get; }
    IDbSet<LoadingDocumentResource> LoadingDocumentResources { get; }
    IDbSet<LoadingDocument> LoadingDocuments { get; }
    IDbSet<MeasureUnit> MeasureUnits { get; }
    IDbSet<Resource> Resources { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}