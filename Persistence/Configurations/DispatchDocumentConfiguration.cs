using Domain.Entities.DispatchDocuments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

file sealed class DispatchDocumentConfiguration : IEntityTypeConfiguration<DispatchDocument>
{
    public void Configure(EntityTypeBuilder<DispatchDocument> builder)
    {
        builder.HasIndex(b => b.DocumentNumber).IsUnique();
    }
}