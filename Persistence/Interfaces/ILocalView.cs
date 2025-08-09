namespace Persistence.Interfaces;

public interface ILocalView<out TEntity> : IEnumerable<TEntity> where TEntity : class;