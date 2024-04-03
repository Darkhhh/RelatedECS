using RelatedECS.Entities;
using RelatedECS.Maintenance.Utilities;
using RelatedECS.Pools;

namespace RelatedECS.Tests.Dummies;

internal class DisabledWorldDummy : IWorld
{
    public IMessageBus Bus => new MessageBus(this);

    public ISharedBag Bag => new SharedBag();

    public IEntity GetEntityById(int id)
    {
        return null;
    }

    public ComponentsPool<T> GetPool<T>() where T : struct
    {
        return null;
    }

    public IComponentsPool GetPool(Type type)
    {
        return null;
    }

    public IEntity NewEntity()
    {
        return null;
    }

    public EntityPack PackEntity(IEntity entity)
    {
        return default;
    }
}
