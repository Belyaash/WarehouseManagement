using Domain.Entities.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

file sealed class ResourceConfiguration : IEntityTypeConfiguration<Resource>
{
    public void Configure(EntityTypeBuilder<Resource> builder)
    {
        builder.HasIndex(b => b.Name).IsUnique();

        builder.HasMany(b => b.DispatchDocumentResources)
            .WithOne(d => d.Resource)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(b => b.LoadingDocumentResources)
            .WithOne(d => d.Resource)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(b => b.Balances)
            .WithOne(d => d.Resource)
            .OnDelete(DeleteBehavior.Restrict);
    }
}