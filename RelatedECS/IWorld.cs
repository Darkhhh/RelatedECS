using RelatedECS.Entities;
using RelatedECS.Filters;
using RelatedECS.Maintenance.Utilities;
using RelatedECS.Pools;
using RelatedECS.Systems.SystemGroups;
using RelatedECS.Systems;

namespace RelatedECS;

public interface IWorld
{
    public IWorld AddSystem(ISystem system);

    public IWorld AddGroup(ISystemGroup systemGroup);

    public ISystemGroup GetSystemGroup(string name);

    public IWorld Prepare();

    public void Execute();

    public void Dispose();

    public EntitiesFilter RegisterFilter(IFilterDeclaration declaration);


    public ComponentsPool<T> GetPool<T>() where T : struct;

    public IComponentsPool GetPool(Type type);


    public IEntity NewEntity();

    public EntityPack PackEntity(IEntity entity);

    public IEntity GetEntityById(int id);


    public IMessageBus Bus { get; }

    public ISharedBag Bag { get; }
}
