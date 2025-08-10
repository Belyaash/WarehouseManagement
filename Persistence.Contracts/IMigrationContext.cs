namespace Persistence.Contracts;

public interface IMigrationContext
{
    Task MigrateAsync(CancellationToken cancellationToken = default);
}