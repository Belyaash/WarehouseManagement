using Domain.Entities.DomainClients;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

file sealed class DomainClientConfiguration : IEntityTypeConfiguration<DomainClient>
{
    public void Configure(EntityTypeBuilder<DomainClient> builder)
    {
        builder.HasIndex(b => b.Name).IsUnique();

        builder.HasMany(b => b.DispatchDocuments)
            .WithOne(b => b.Client)
            .OnDelete(DeleteBehavior.Cascade);
    }
}