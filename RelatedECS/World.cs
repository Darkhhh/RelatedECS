using RelatedECS.Entities;
using RelatedECS.Filters;
using RelatedECS.Filters.Conditions;
using RelatedECS.Maintenance.Utilities;
using RelatedECS.Pools;
using RelatedECS.Systems;

namespace RelatedECS;

public class World : IWorld
{
    private readonly ComponentsPoolsController _componentsPoolsController;
    private readonly EntitiesController _entitiesController;
    private readonly FiltersController _filtersController;

    private readonly MessageBus _messageBus;
    private readonly SharedBag _sharedBag;

    public IMessageBus Bus => _messageBus;
    public ISharedBag Bag => _sharedBag;

    public World()
    {
        _componentsPoolsController = new ComponentsPoolsController(PoolUpdated);
        _entitiesController = new EntitiesController(this);
        _filtersController = new FiltersController(_entitiesController, _componentsPoolsController);

        _messageBus = new MessageBus(this);
        _sharedBag = new SharedBag();
    }

    public ComponentsPool<T> GetPool<T>() where T : struct
    {
        var pool = _componentsPoolsController.GetPool<T>();
        _entitiesController.UpdatePoolsAmount(_componentsPoolsController.PoolsCount);
        _filtersController.ResizeMasks(_componentsPoolsController.PoolsCount);

        return pool;
    }

    public IComponentsPool GetPool(Type type) => _componentsPoolsController.GetPool(type);

    public IEntity NewEntity() => _entitiesController.New();

    public EntityPack PackEntity(IEntity entity) => _entitiesController.Pack(entity);

    public IEntity GetEntityById(int id) => _entitiesController.GetById(id);


    internal EntitiesFilter RegisterFilter(IFilterDeclaration declaration)
    {
        var filter = _filtersController.RegisterAsEntitiesFilter(declaration);
        _entitiesController.UpdatePoolsAmount(_componentsPoolsController.PoolsCount);
        _filtersController.ResizeMasks(_componentsPoolsController.PoolsCount);
        return filter;
    }

    internal EntitiesConditionedFilter RegisterAsConditionedFilter(IFilterDeclaration declaration)
    {
        var filter = _filtersController.RegisterAsConditionedFilter(declaration);
        _entitiesController.UpdatePoolsAmount(_componentsPoolsController.PoolsCount);
        _filtersController.ResizeMasks(_componentsPoolsController.PoolsCount);
        return filter;
    }

    private void PoolUpdated(Type poolType, int poolIndex, int entity, bool added)
    {
        _entitiesController.PoolUpdated(poolType, poolIndex, entity, added);
        _filtersController.PoolUpdated(poolType, poolIndex, entity, added);
    }
}