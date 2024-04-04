using RelatedECS.Entities;
using RelatedECS.Filters;
using RelatedECS.Maintenance.Utilities;
using RelatedECS.Pools;
using RelatedECS.Systems;
using RelatedECS.Systems.SystemGroups;

namespace RelatedECS;

public class World : IWorld
{
    private readonly ComponentsPoolsController _componentsPoolsController;
    private readonly EntitiesController _entitiesController;
    private readonly FiltersController _filtersController;
    private readonly ISystemsController _systemsController;
    
    private readonly MessageBus _messageBus;
    private readonly SharedBag _sharedBag;

    public IMessageBus Bus => _messageBus;
    public ISharedBag Bag => _sharedBag;


    public World()
    {
        _componentsPoolsController = new ComponentsPoolsController(PoolUpdated);
        _entitiesController = new EntitiesController(this);
        _filtersController = new FiltersController(_entitiesController, _componentsPoolsController);
        _systemsController = new SystemsController();

        _messageBus = new MessageBus(this);
        _sharedBag = new SharedBag();
    }

    public IWorld AddSystem(ISystem system)
    {
        _systemsController.AddSystem(system);
        return this;
    }

    public IWorld AddGroup(ISystemGroup systemGroup)
    {
        _systemsController.AddGroup(systemGroup);
        return this;
    }

    public ISystemGroup GetSystemGroup(string name)
    {
        return _systemsController.GetSystemGroup(name);
    }

    public IWorld Prepare()
    {
        _systemsController.Prepare(this);
        return this;
    }

    public void Execute()
    {
        _systemsController.FramePrepare(this);
        _systemsController.Execute(this);
        _systemsController.LateExecute(this);
        _systemsController.FrameDispose(this);
    }

    public void Dispose()
    {
        _systemsController.Dispose(this);
    }


    public EntitiesFilter RegisterFilter(IFilterDeclaration declaration)
    {
        var filter = _filtersController.RegisterAsEntitiesFilter(declaration);
        _entitiesController.UpdatePoolsAmount(_componentsPoolsController.PoolsCount);
        _filtersController.ResizeMasks(_componentsPoolsController.PoolsCount);
        return filter;
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


    private void PoolUpdated(Type poolType, int poolIndex, int entity, bool added)
    {
        _entitiesController.PoolUpdated(poolType, poolIndex, entity, added);
        _filtersController.PoolUpdated(poolType, poolIndex, entity, added);
    }
}
