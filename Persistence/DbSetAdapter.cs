using System.Collections;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Persistence.Contracts;

namespace Persistence;

internal sealed class DbSetAdapter<TEntity>(DbContext context) :
    IDbSet<TEntity>
    where TEntity : class
{
    private readonly LocalViewAdapter<TEntity> _local = new(context);
    private readonly DbSet<TEntity> _set = context.Set<TEntity>();

    public IEnumerator<TEntity> GetEnumerator()
    {
        return ((IEnumerable<TEntity>) _set).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public Type ElementType => ((IQueryable<TEntity>) _set).ElementType;

    public Expression Expression => ((IQueryable<TEntity>) _set).Expression;

    public IQueryProvider Provider => ((IQueryable<TEntity>) _set).Provider;

    public ILocalView<TEntity> Local => _local;

    public void Add(TEntity entity)
    {
        _set.Add(entity);
    }

    public void AddRange(IEnumerable<TEntity> entities)
    {
        _set.AddRange(entities);
    }

    public void Remove(TEntity entity)
    {
        _set.Remove(entity);
    }

    public void RemoveRange(IEnumerable<TEntity> entities)
    {
        _set.RemoveRange(entities);
    }

    public void Update(TEntity entity)
    {
        _set.Update(entity);
    }

    public void UpdateRange(IEnumerable<TEntity> entities)
    {
        _set.UpdateRange(entities);
    }

    public IAsyncEnumerator<TEntity> GetAsyncEnumerator(CancellationToken cancellationToken = new())
    {
        return _set.GetAsyncEnumerator(cancellationToken);
    }
}