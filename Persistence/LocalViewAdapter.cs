using System.Collections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Persistence.Contracts;

namespace Persistence;

internal sealed class LocalViewAdapter<TEntity>(DbContext context) : ILocalView<TEntity>
    where TEntity : class
{
    private readonly LocalView<TEntity> _localView = context.Set<TEntity>().Local;

    public IEnumerator<TEntity> GetEnumerator()
    {
        return _localView.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}