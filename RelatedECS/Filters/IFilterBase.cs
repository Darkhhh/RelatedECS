using RelatedECS.Entities;
using RelatedECS.Pools;

namespace RelatedECS.Filters;

public interface IFilterBase<TReturnType> : IEnumerable<TReturnType>, IEnumerator<TReturnType>
{
    public int Count { get; }

    public void FillRawCollection(in List<TReturnType> rawEntities);

    public bool HasPool<T>() where T : IComponentsPool;

    public T GetPool<T>() where T : IComponentsPool;
}

public interface IEntitiesFilter : IFilterBase<Entity>;

public interface IIndicesFilter : IFilterBase<int>;
