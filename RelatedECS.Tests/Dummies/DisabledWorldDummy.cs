using RelatedECS.Entities;
using RelatedECS.Filters;
using RelatedECS.Maintenance.Utilities;
using RelatedECS.Pools;
using RelatedECS.Systems;
using RelatedECS.Systems.SystemGroups;

namespace RelatedECS.Tests.Dummies;

internal class DisabledWorldDummy : IWorld
{
    public IMessageBus Bus => new MessageBus(this);

    public ISharedBag Bag => new SharedBag();

    public IWorld AddGroup(ISystemGroup systemGroup)
    {
        return this;
    }

    public IWorld AddSystem(ISystem system)
    {
        return this;
    }

    public void Dispose()
    {
        
    }

    public void Execute()
    {
        
    }

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

    public ISystemGroup GetSystemGroup(string name)
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

    public IWorld Prepare()
    {
        return this;
    }

    public EntitiesFilter RegisterFilter(IFilterDeclaration declaration)
    {
        return null;
    }
}
