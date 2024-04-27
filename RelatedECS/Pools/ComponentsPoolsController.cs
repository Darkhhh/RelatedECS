using RelatedECS.Maintenance.Utilities;

namespace RelatedECS.Pools;

public class ComponentsPoolsController(PoolUpdated action) : IComponentsPoolsController
{
    private readonly Dictionary<Type, IComponentsPool> _pools = new();

    public int PoolsCount => _pools.Count;

    public PoolUpdated PoolUpdated => action;

    public IReadOnlyList<IComponentsPool> GetAll() => _pools.Values.ToList();

    public (Mask With, Mask Without) GetMasks(Type[] withTypes, Type[] withoutTypes)
    {
        var with = new Mask();
        foreach (var type in withTypes)
        {
            var pool = CreatePoolOfType(type);
            with.Resize(PoolsCount);
            with.Set(pool.Id, true);
        }

        var without = new Mask();
        foreach (var type in withoutTypes)
        {
            var pool = CreatePoolOfType(type);
            without.Resize(PoolsCount);
            without.Set(pool.Id, true);
        }

        return (with, without);
    }

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

    public IComponentsPool CreatePoolOfType(Type componentType)
    {
        if (_pools.TryGetValue(componentType, out var p)) return p;

        string msg = $"Could not create pool with {componentType.Name}";
        Type type = typeof(ComponentsPool<>).MakeGenericType(componentType);

        var method = GetType().GetMethod(nameof(PoolBeenChanged),
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Default | System.Reflection.BindingFlags.Instance);
        var del = Delegate.CreateDelegate(typeof(Action<Type, int, bool>), this, method!);

        object pool = Activator.CreateInstance(type, new object[] { _pools.Count, del }) ?? throw new NullReferenceException(msg);
        var cast = pool as IComponentsPool ?? throw new NullReferenceException(msg);
        _pools.Add(componentType, cast);
        return cast;
    }

    private void PoolBeenChanged(Type poolType, int entity, bool added) => PoolUpdated(poolType, _pools[poolType].Id, entity, added);
}