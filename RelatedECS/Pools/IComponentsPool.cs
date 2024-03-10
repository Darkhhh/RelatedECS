namespace RelatedECS.Pools;

public interface IComponentsPool
{
    protected const int InitialCapacity = 256;

    public int Id { get; }

    public bool Has(int entity);

    public void Delete(int entity);

    public Action<Type, int, bool> PoolChanged { get; init; }
}
