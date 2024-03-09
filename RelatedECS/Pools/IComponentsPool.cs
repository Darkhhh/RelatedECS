namespace RelatedECS.Pools;

public interface IComponentsPool
{
    public int Id { get; }

    public bool Has(int entity);

    public void Delete(int entity);
}
