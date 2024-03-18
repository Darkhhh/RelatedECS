namespace RelatedECS.Entities;

internal interface IEntitiesController
{
    public IEntity New();

    public IEnumerable<IEntity> GetEntitiesRaw();

    public IEntity GetById(int id);

    public void UpdatePoolsAmount(int poolsAmount);

    public void PoolUpdated(Type poolType, int poolIndex, int entity, bool added);

    public EntityPack Pack(IEntity entity);
}
