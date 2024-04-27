using RelatedECS.Entities;
using RelatedECS.Maintenance.Filters;
using RelatedECS.Maintenance.Utilities;
using RelatedECS.Pools;
using System.Diagnostics;

namespace RelatedECS.Filters.Conditions;

public class EntitiesConditionedFilter : EntitiesFilter
{
    private readonly IPoolsProvider _poolsProvider;
    private readonly RegisteredFilter _filter;
    private readonly ObjectPool<ConditionedEntitiesEnumerator> _enumeratorsPool;

    internal EntitiesConditionedFilter(RegisteredFilter filter, IFilterDeclaration declaration, IPoolsProvider provider) : base(filter, declaration)
    {
        _filter = filter;
        _poolsProvider = provider;
        _enumeratorsPool = new ObjectPool<ConditionedEntitiesEnumerator>(EnumeratorGenerator);
    }

    public IEnumerable<IEntity> Entities(params IConditionCheck[] checks)
    {
        foreach (var condition in checks)
        {
            if (!Declaration.HasWithType(condition.ObjectType) && !Declaration.HasWithoutType(condition.ObjectType))
                throw new Exception($"Declaration of filter does not contain {condition.ObjectType.Name} component type");
#if DEBUG
            Debug.Assert(checks.Select(t => t.ObjectType).ToHashSet().Count == checks.Length);
#endif
        }
        var enumerator = _enumeratorsPool.Get();
        enumerator.ClearChecks();
        foreach (var condition in checks) enumerator.Add(condition);
        _filter.Lock();
        return enumerator;
    }

    private ConditionedEntitiesEnumerator EnumeratorGenerator()
    {
        var enumerator = new ConditionedEntitiesEnumerator(_filter, _poolsProvider, e =>
        {
            _filter.Unlock();
            _enumeratorsPool.Return((ConditionedEntitiesEnumerator)e);
        });
        return enumerator;
    }
}