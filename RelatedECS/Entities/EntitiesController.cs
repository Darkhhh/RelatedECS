﻿using RelatedECS.Maintenance.Utilities;

namespace RelatedECS.Entities;

public class EntitiesController : IEntitiesController
{
    private readonly IWorld _world;
    private readonly ObjectPool<Entity> _recycledEntities;
    private readonly Dictionary<int, Entity> _entities = new();

    private int _currentEntityIndex = 0;
    private int _lastPoolsCount;

    public EntitiesController(IWorld world)
    {
        _recycledEntities = new(NewEntityGenerator);
        _world = world;
    }

    public bool IsAlive(IEntity entity) => entity.IsAlive;

    public void PoolUpdated(Type poolType, int poolIndex, int entity, bool added)
    {
        var wrap = GetById(entity);
        var mask = wrap.GetMask();

        mask.Set(poolIndex, added);
        if (mask.IsEmpty) _recycledEntities.Return((Entity)wrap);
    }

    public void UpdatePoolsAmount(int poolsAmount)
    {
        _lastPoolsCount = poolsAmount;
        foreach (var entity in _entities.Values)
        {
            entity.GetMask().Resize(_lastPoolsCount);
        }
    }

    public IEntity GetById(int id)
    {
        if (_entities.TryGetValue(id, out var entity) && entity.IsAlive) return entity;
        throw new InvalidOperationException("Could not get entity by id or it is not alive");
    }

    public IEnumerable<IEntity> GetEntitiesRaw() => _entities.Values;

    public IEntity New() => _recycledEntities.Get();
    private Entity NewEntityGenerator()
    {
        var e = new Entity(_currentEntityIndex++, _world);
        e.GetMask().Resize(_lastPoolsCount);
        return e;
    }
}
