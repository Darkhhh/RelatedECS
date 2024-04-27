using RelatedECS.Entities;
using RelatedECS.Maintenance.Utilities;
using RelatedECS.Pools;

namespace RelatedECS;

public interface IWorld
{
    public ComponentsPool<T> GetPool<T>() where T : struct;

    public IComponentsPool GetPool(Type type);

    public IEntity NewEntity();

    public EntityPack PackEntity(IEntity entity);

    public IEntity GetEntityById(int id);

    public IMessageBus Bus { get; }

    public ISharedBag Bag { get; }
}