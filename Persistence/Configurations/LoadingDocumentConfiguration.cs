using Domain.Entities.LoadingDocuments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

file sealed class LoadingDocumentConfiguration : IEntityTypeConfiguration<LoadingDocument>
{
    public void Configure(EntityTypeBuilder<LoadingDocument> builder)
    {
        builder.HasIndex(x => x.DocumentNumber).IsUnique();
    }
}