namespace RelatedECS.Pools;

public class ComponentsPool<T> : IComponentsPool where T : struct
{
    public int Id { get; }

    private readonly Type _componentType = typeof(T);

    public Action<Type, int, bool> PoolChanged { get; init; }

    private Component[] _components = new Component[IComponentsPool.InitialCapacity];
    private int[] _componentsByEntities = new int[IComponentsPool.InitialCapacity];
    private int[] _recycledComponentsIndices = new int[IComponentsPool.InitialCapacity];
    private int _recycledComponentsAmount = 0;
    private int _size;

    private AutoResetHandle<T>? _resetHandler;

    public ComponentsPool(int id, Action<Type, int, bool> poolChanged)
    {
        PoolChanged = poolChanged;
        Id = id;
        Init();
    }
    private void Init()
    {
        _recycledComponentsAmount = IComponentsPool.InitialCapacity;
        for (var i = 0; i < _recycledComponentsAmount; i++) _recycledComponentsIndices[i] = i;
        Array.Fill(_componentsByEntities, -1);
        Array.Fill(_components, new Component { Entity = int.MinValue, Value = default });

        if (_componentType.IsAssignableTo(typeof(IAutoResetComponent<T>)))
        {
            var method = typeof(IAutoResetComponent<T>).GetMethod(nameof(IAutoResetComponent<T>.AutoReset));
            if (method is null) throw new ArgumentNullException(nameof(method));
            _resetHandler = (AutoResetHandle<T>)Delegate.CreateDelegate(typeof(AutoResetHandle<T>), method);
        }
    }

    public ref T Add(int entity)
    {
        if (Has(entity)) throw new Exception("Entity already in pool");
        if (_componentsByEntities.Length < entity) Array.Resize(ref _componentsByEntities, entity + 1);
        int componentIndex;

        if (_recycledComponentsAmount > 0)
        {
            componentIndex = _recycledComponentsIndices[--_recycledComponentsAmount];
            ref var component = ref _components[componentIndex];
            component.Entity = entity;
        }
        else
        {
            if (_size + 1 >= _components.Length) Array.Resize(ref _components, _components.Length * 2);
            componentIndex = _size++;
            _components[componentIndex] = new Component { Entity = entity, Value = default };
        }

        _componentsByEntities[entity] = componentIndex;

        _resetHandler?.Invoke(ref _components[componentIndex].Value);

        PoolChanged.Invoke(_componentType, entity, true);
        return ref _components[componentIndex].Value;
    }

    public bool Has(int entity)
    {
        return entity < _componentsByEntities.Length
               && _componentsByEntities[entity] != -1
               && _components[_componentsByEntities[entity]].Entity == entity;
    }

    public ref T Get(int entity)
    {
        if (!Has(entity)) throw new Exception($"Trying to access not created component with entity id:{entity}");
        return ref _components[_componentsByEntities[entity]].Value;
    }

    public void Delete(int entity)
    {
        if (!Has(entity)) return;
        ref var component = ref _components[_componentsByEntities[entity]];
#if DEBUG
        if (component.Entity != entity) throw new Exception("Incorrect component");
#endif
        component.Entity = int.MinValue;
        component.Value = default;
        _resetHandler?.Invoke(ref component.Value);
        _recycledComponentsAmount++;
        if (_recycledComponentsIndices.Length <= _recycledComponentsAmount + 1)
            Array.Resize(ref _recycledComponentsIndices, _recycledComponentsAmount * 2);
        _recycledComponentsIndices[_recycledComponentsAmount] = _componentsByEntities[entity];
        _componentsByEntities[entity] = -1;

        PoolChanged.Invoke(_componentType, entity, false);
    }

    private struct Component
    {
        public int Entity;
        public T Value;
    }
}
