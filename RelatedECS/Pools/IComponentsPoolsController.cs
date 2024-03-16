namespace RelatedECS.Pools;

public delegate void PoolUpdated(Type poolType, int poolIndex, int entity, bool added);

public interface IComponentsPoolsController
{
    public int PoolsCount { get; }

    public PoolUpdated PoolUpdated { get; }

    public ComponentsPool<T> GetPool<T>() where T : struct;

    public IComponentsPool GetPool(Type type);

    public IReadOnlyList<IComponentsPool> GetAll();
}
