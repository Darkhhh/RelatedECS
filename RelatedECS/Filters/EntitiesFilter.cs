using RelatedECS.Entities;
using RelatedECS.Maintenance.Filters;
using RelatedECS.Maintenance.Utilities;

namespace RelatedECS.Filters;

public class EntitiesFilter : IFilter<IEntity>
{
    private readonly ObjectPool<EntitiesEnumerator> _enumeratorsPool;
    private readonly RegisteredFilter _filter;

    internal IFilterDeclaration Declaration { get; }

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
