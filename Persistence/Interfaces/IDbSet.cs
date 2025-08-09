namespace Persistence.Interfaces;

public interface IDbSet<TEntity> : IQueryable<TEntity>, IAsyncEnumerable<TEntity> where TEntity : class
{
    ILocalView<TEntity> Local { get; }
    
    void Add(TEntity entity);

    void AddRange(IEnumerable<TEntity> entities);

    void Remove(TEntity entity);

    void RemoveRange(IEnumerable<TEntity> entities);

    void Update(TEntity entity);
    
    void UpdateRange(IEnumerable<TEntity> entities);
}