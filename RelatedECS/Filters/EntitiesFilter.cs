using RelatedECS.Entities;
using RelatedECS.Maintenance.Filters;
using RelatedECS.Maintenance.Utilities;
using System.Diagnostics.CodeAnalysis;

namespace RelatedECS.Filters;

public class EntitiesFilter : IFilter<IEntity>
{
    private readonly ObjectPool<EntitiesEnumerator> _enumeratorsPool;
    private readonly RegisteredFilter _filter;

    internal IFilterDeclaration Declaration { get; }

    internal RegisteredFilter Raw => _filter;

    internal EntitiesFilter(RegisteredFilter filter, IFilterDeclaration declaration)
    {
        _filter = filter;
        Declaration = declaration;
        _enumeratorsPool = new ObjectPool<EntitiesEnumerator>(EnumeratorGenerator);
    }

    public int Count => _filter.Count;

    public IEnumerable<IEntity> Entities()
    {
        var enumerator = _enumeratorsPool.Get();
        _filter.Lock();
        return enumerator;
    }

    public bool TryGetFirst([MaybeNullWhen(false)] out IEntity entity)
    {
        entity = _filter.Get(0) ?? default;
        return entity is not null;
    }

    private EntitiesEnumerator EnumeratorGenerator()
    {
        var enumerator = new EntitiesEnumerator(_filter, e =>
        {
            _filter.Unlock();
            _enumeratorsPool.Return((EntitiesEnumerator)e);
        });
        return enumerator;
    }
}