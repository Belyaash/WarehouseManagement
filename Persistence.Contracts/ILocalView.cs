namespace Persistence.Contracts;

public interface ILocalView<out TEntity> : IEnumerable<TEntity> where TEntity : class;