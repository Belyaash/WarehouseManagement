using Domain.Entities.MeasureUnits;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

file sealed class MeasureUnitConfiguration : IEntityTypeConfiguration<MeasureUnit>
{
    public void Configure(EntityTypeBuilder<MeasureUnit> builder)
    {
        builder.HasIndex(b => b.Name).IsUnique();

        builder.HasMany(b => b.DispatchDocumentResources)
            .WithOne(d => d.MeasureUnit)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(b => b.LoadingDocumentResources)
            .WithOne(d => d.MeasureUnit)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(b => b.Balances)
            .WithOne(d => d.MeasureUnit)
            .OnDelete(DeleteBehavior.Restrict);
    }
}