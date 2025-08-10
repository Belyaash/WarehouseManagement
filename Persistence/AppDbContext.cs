using System.Reflection;
using Domain.Entities.Balances;
using Domain.Entities.DispatchDocumentResources;
using Domain.Entities.DispatchDocuments;
using Domain.Entities.DomainClients;
using Domain.Entities.LoadingDocumentResources;
using Domain.Entities.LoadingDocuments;
using Domain.Entities.MeasureUnits;
using Domain.Entities.Resources;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options)
    : DbContext(options)
{
    public DbSet<Balance> Balances => Set<Balance>();
    public DbSet<DispatchDocumentResource> DispatchDocumentResources => Set<DispatchDocumentResource>();
    public DbSet<DispatchDocument> DispatchDocuments => Set<DispatchDocument>();
    public DbSet<DomainClient> DomainClients => Set<DomainClient>();
    public DbSet<LoadingDocumentResource> LoadingDocumentResources => Set<LoadingDocumentResource>();
    public DbSet<LoadingDocument> LoadingDocuments => Set<LoadingDocument>();
    public DbSet<MeasureUnit> MeasureUnits => Set<MeasureUnit>();
    public DbSet<DomainResource> Resources => Set<DomainResource>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }
}