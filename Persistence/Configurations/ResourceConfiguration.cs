using Domain.Entities.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

file sealed class ResourceConfiguration : IEntityTypeConfiguration<DomainResource>
{
    public void Configure(EntityTypeBuilder<DomainResource> builder)
    {
        builder.HasIndex(b => b.Name).IsUnique();

        builder.HasMany(b => b.DispatchDocumentResources)
            .WithOne(d => d.DomainResource)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(b => b.LoadingDocumentResources)
            .WithOne(d => d.DomainResource)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(b => b.Balances)
            .WithOne(d => d.DomainResource)
            .OnDelete(DeleteBehavior.Restrict);
    }
}