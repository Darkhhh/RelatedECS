namespace RelatedECS.Pools;

public class ComponentsPoolsController(PoolUpdated action) : IComponentsPoolsController
{
    private readonly Dictionary<Type, IComponentsPool> _pools = new();

    public int PoolsCount => _pools.Count;

    public PoolUpdated PoolUpdated => action;

    public IReadOnlyList<IComponentsPool> GetAll() => _pools.Values.ToList();

    public ComponentsPool<T> GetPool<T>() where T : struct
    {
        var type = typeof(T);
        if (_pools.TryGetValue(type, out var value)) return (ComponentsPool<T>)value;

        var pool = new ComponentsPool<T>(_pools.Count, PoolBeenChanged);
        _pools.Add(type, pool);
        return pool;
    }

    public IComponentsPool GetPool(Type type)
    {
        if (!_pools.TryGetValue(type, out var value)) throw new Exception("Pool is not registered");
        return value;
    }

    private void PoolBeenChanged(Type poolType, int entity, bool added) => PoolUpdated(poolType, _pools[poolType].Id, entity, added);
}
