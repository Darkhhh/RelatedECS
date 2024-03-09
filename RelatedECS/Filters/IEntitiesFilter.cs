using RelatedECS.Entities;
using RelatedECS.Pools;

namespace RelatedECS.Filters;

public interface IEntitiesFilter : IEnumerable<Entity>, IEnumerator<Entity>
{
    public int Count { get; }

    public void FillRawCollection(in List<Entity> rawEntities);

    public bool HasPool<T>() where T : IComponentsPool;

    public T GetPool<T>() where T : IComponentsPool;

    public IEntitiesFilter With<T>() where T : struct;

    public IEntitiesFilter Without<T>() where T : struct;
}
