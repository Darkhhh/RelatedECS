using RelatedECS.Entities;
using RelatedECS.Filters;
using RelatedECS.Maintenance.Utilities;
using RelatedECS.Pools;

namespace RelatedECS;

public interface IWorld
{
    public ComponentsPool<T> GetPool<T>() where T : struct;

    public IEntity NewEntity();

    public IMessageBus Bus { get; }

    public ISharedBag Bag { get; }

    public IEntitiesFilter RegisterAsEntitiesFilter(IFilterDeclaration declaration);
    public IIndicesFilter RegisterAsIndicesFilter(IFilterDeclaration declaration);
}
