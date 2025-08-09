using Domain.Entities.Balances;
using Domain.Entities.DispatchDocumentResources;
using Domain.Entities.DispatchDocuments;
using Domain.Entities.DomainClients;
using Domain.Entities.LoadingDocumentResources;
using Domain.Entities.LoadingDocuments;
using Domain.Entities.MeasureUnits;
using Domain.Entities.Resources;
using Microsoft.EntityFrameworkCore;
using Persistence.Interfaces;

namespace Persistence;

internal sealed class DbContextAdapter(DbContext context) : IDbContext, ITransactionContext, IMigrationContext,
    IAsyncDisposable,
    IDisposable
{
    public IDbSet<Balance> Balances { get; } = new DbSetAdapter<Balance>(context);
    public IDbSet<DispatchDocumentResource> DispatchDocumentResources { get; } = new DbSetAdapter<DispatchDocumentResource>(context);
    public IDbSet<DispatchDocument> DispatchDocuments { get; } = new DbSetAdapter<DispatchDocument>(context);
    public IDbSet<DomainClient> DomainClients { get; } = new DbSetAdapter<DomainClient>(context);
    public IDbSet<LoadingDocumentResource> LoadingDocumentResources { get; } = new DbSetAdapter<LoadingDocumentResource>(context);
    public IDbSet<LoadingDocument> LoadingDocuments { get; } = new DbSetAdapter<LoadingDocument>(context);
    public IDbSet<MeasureUnit> MeasureUnits { get; } = new DbSetAdapter<MeasureUnit>(context);
    public IDbSet<Resource> Resources { get; } = new DbSetAdapter<Resource>(context);

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return context.SaveChangesAsync(cancellationToken);
    }

    public Task ExecuteSql(string sql)
    {
        return context.Database.ExecuteSqlRawAsync(sql);
    }

    public async ValueTask DisposeAsync()
    {
        await context.DisposeAsync();
    }

    public void Dispose()
    {
        context.Dispose();
    }

    public Task MigrateAsync(CancellationToken cancellationToken)
    {
        return context.Database.MigrateAsync(cancellationToken);
    }

    public Task BeginTransactionAsync(CancellationToken cancellationToken)
    {
        return context.Database.BeginTransactionAsync(cancellationToken);
    }

    public Task CommitTransactionAsync(CancellationToken cancellationToken)
    {
        return context.Database.CommitTransactionAsync(cancellationToken);
    }

    public Task RollbackTransactionAsync(CancellationToken cancellationToken)
    {
        return context.Database.RollbackTransactionAsync(cancellationToken);
    }

    public void RemoveRange(IEnumerable<object> entities)
    {
        context.RemoveRange(entities);
    }
}