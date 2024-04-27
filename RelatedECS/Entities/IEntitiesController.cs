namespace RelatedECS.Entities;

internal interface IEntitiesStorage
{
    public IEnumerable<IEntity> GetEntitiesRaw();

    public IEntity GetById(int id);
}

internal interface IEntitiesController : IEntitiesStorage
{
    public IEntity New();

    public void UpdatePoolsAmount(int poolsAmount);

    public void PoolUpdated(Type poolType, int poolIndex, int entity, bool added);

    public EntityPack Pack(IEntity entity);
}