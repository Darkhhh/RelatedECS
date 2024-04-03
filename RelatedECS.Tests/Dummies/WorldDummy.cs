using RelatedECS.Entities;
using RelatedECS.Filters;
using RelatedECS.Maintenance.Utilities;
using RelatedECS.Pools;

namespace RelatedECS.Tests.Dummies;

internal class WorldDummy : IWorld
{
    private readonly IComponentsPoolsController _componentsPoolsController;
    private readonly EntitiesController _entitiesController;

    public IMessageBus Bus { get; }
    public ISharedBag Bag { get; }
    public EntitiesController EntitiesController => _entitiesController;
    public IComponentsPoolsController PoolsController => _componentsPoolsController;

    public WorldDummy()
    {
        _componentsPoolsController = new ComponentsPoolsController(PoolUpdated);
        _entitiesController = new EntitiesController(this);
        Bus = new MessageBus(this);
        Bag = new SharedBag();
    }

    public IEntity NewEntity() => _entitiesController.New();

    public ComponentsPool<T> GetPool<T>() where T : struct
    {
        var pool = _componentsPoolsController.GetPool<T>();
        _entitiesController.UpdatePoolsAmount(_componentsPoolsController.PoolsCount);

        return pool;
    }

    private void PoolUpdated(Type poolType, int poolIndex, int entity, bool added)
    {
        _entitiesController.PoolUpdated(poolType, poolIndex, entity, added);
    }

    public IComponentsPool GetPool(Type type) => _componentsPoolsController.GetPool(type);

    public EntityPack PackEntity(IEntity entity) => _entitiesController.Pack(entity);

    public IEntity GetEntityById(int id) => _entitiesController.GetById(id);
}
