namespace Persistence.Interfaces;

public interface IMigrationContext
{
    Task MigrateAsync(CancellationToken cancellationToken = default);
}